using System;

namespace Moonshine.Data.Infrastructure;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class,  Inherited = false)]
public class NoTransactionAttribute : Attribute
{
}