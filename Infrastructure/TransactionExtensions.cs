using System.Linq;
using System.Reflection;

namespace Moonshine.Data.Infrastructure;

public static class TransactionExtensions
{
    public static bool IsExcludedFromGlobalTransactions(this ICustomAttributeProvider provider)
    {
        return provider?.GetCustomAttributes(false).Any(x =>
            x.GetType() == typeof(NoTransactionAttribute) || x.GetType() == typeof(TransactionalAttribute)) ?? false;
    }
    
    public static bool IsExcludedFromTransactions(this ICustomAttributeProvider provider)
    {
        return provider?.GetCustomAttributes(typeof(NoTransactionAttribute), false).Any() ?? false;
    }
}