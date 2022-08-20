using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Moonshine.Data.Repositories.Interfaces;

namespace Moonshine.Data.Infrastructure;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class TransactionRunnerAttribute : Attribute, IAsyncPageFilter
{
    private IUnitOfWork _unitOfWork;

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        _unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
        return Task.CompletedTask;
    }

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        var attr = GetAttribute(context);
        if (attr == null)
        {
            await next();
            return;
        }

        var transactional = (TransactionalAttribute) attr;
        var statusesToRollback = transactional.StatusesToRollbackTransaction;
        var handler = await _unitOfWork.EnsureTransactionAsync(transactional.IsolationLevel);
        
        var result = await next();
        if (result.Result != null)
        {
            await result.Result.ExecuteResultAsync(context);
        }
        
        if (result.Exception == null && statusesToRollback.All(x => (int)x != result.HttpContext.Response.StatusCode))
        {
            if (handler.CanBeCommitted)
            {
                await handler.CommitAsync(default);
            }
        }
        else
        {
            await handler.RollbackAsync(default);
        }
    }

    private static object GetAttribute(PageHandlerExecutingContext context)
    {
        var methodAttr = context.HandlerMethod?.MethodInfo
            .GetCustomAttributes(typeof(TransactionalAttribute), false).FirstOrDefault();

        var classAttr = context.HandlerMethod?.MethodInfo.DeclaringType
            ?.GetCustomAttributes(typeof(TransactionalAttribute), false).FirstOrDefault();

        if (classAttr == null && methodAttr == null)
        {
            return null;
        }

        // So transaction was defined on class level not on method level, so it can be bypassed by
        // NoTransactionAttribute on method level
        if (methodAttr == null)
        {
            if (context.HandlerMethod?.MethodInfo.IsExcludedFromTransactions() ?? false)
            {
                return null;
            }
        }

        var statuses = new List<HttpStatusCode>();
        var transactionLevel = IsolationLevel.ReadUncommitted;
        
        if (classAttr != null)
        {
            var transactional = (TransactionalAttribute) classAttr;
            transactionLevel = transactional.IsolationLevel;
            statuses.AddRange(transactional.StatusesToRollbackTransaction);
        }
        
        if (methodAttr != null)
        {
            var transactional = (TransactionalAttribute) methodAttr;
            transactionLevel = transactional.IsolationLevel;
            statuses.AddRange(transactional.StatusesToRollbackTransaction);
        }

        return new TransactionalAttribute(statuses, transactionLevel);
    }
}