using System.Threading;
using System.Threading.Tasks;

namespace Moonshine.Core.Security.Cryptography.Contracts;

public interface ICryptoVerificator
{
   ValueTask<bool> Verify(byte[] text, byte[] signature, CancellationToken ct);
   ValueTask<bool> Verify(string text, byte[] signature, CancellationToken ct);
   ValueTask<bool> Verify<T>(T obj, byte[] signature, CancellationToken ct);
}