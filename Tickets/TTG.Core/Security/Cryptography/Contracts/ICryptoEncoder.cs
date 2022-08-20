using System.Threading;
using System.Threading.Tasks;

namespace Moonshine.Core.Security.Cryptography.Contracts;

public interface ICryptoEncoder
{
    ValueTask<byte[]> Encrypt(string text, CancellationToken ct);
}