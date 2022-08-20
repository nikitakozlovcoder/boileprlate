using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moonshine.Core.Caching;

public class AsyncLazy<T> where T : class
{
    private readonly Func<CancellationToken, Task<T>> _factory;
    private readonly T _value = default;

    public AsyncLazy(Func<CancellationToken,  Task<T>> factory)
    {
        _factory = factory;
    }

    public async ValueTask<T> GetValue(CancellationToken ct)
    {
        return  _value ?? await _factory(ct);
    }
}