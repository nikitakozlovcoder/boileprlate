using System.Threading;
using System.Threading.Tasks;

namespace Moonshine.Services.Crypto.Contracts;

public interface IKeysDeliveryService
{
    public Task<string> GetPublicKey(CancellationToken ct);
}