using System.Threading;
using System.Threading.Tasks;

namespace Moonshine.Core.Security.Cryptography.Contracts;

public interface ICryptoDecoder
{
    ValueTask<string> Decrypt(byte[] text, CancellationToken ct);
}