using System.Threading;
using System.Threading.Tasks;

namespace Moonshine.Core.Security.Cryptography.Contracts;

public interface ICryptoSigner
{
    ValueTask<byte[]> Sign(string text, CancellationToken ct);
    ValueTask<byte[]> Sign<T>(T obj, CancellationToken ct);
}