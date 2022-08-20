using System.Threading;
using System.Threading.Tasks;

namespace Moonshine.Core.Security.Cryptography.KeyStore.Contracts;

public interface IKeyStoreProvider<T>
{
    public ValueTask<T> GetKeyInfo(CancellationToken ct = default);
}