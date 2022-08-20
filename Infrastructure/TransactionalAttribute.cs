using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;

namespace Moonshine.Data.Infrastructure;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class,  Inherited = false)]
public class TransactionalAttribute : Attribute
{
    public HttpStatusCode[] StatusesToRollbackTransaction { get; } = { };
    public IsolationLevel IsolationLevel { get; } = IsolationLevel.ReadUncommitted;

    public TransactionalAttribute(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted)
    {
        IsolationLevel = isolationLevel;
    }
    
    public TransactionalAttribute(IEnumerable<HttpStatusCode> codes, IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted)
    {
        IsolationLevel = isolationLevel;
        StatusesToRollbackTransaction = codes.ToArray();
    }
    
    public TransactionalAttribute(params HttpStatusCode[] codes)
    {
        StatusesToRollbackTransaction = codes;
    }
}
