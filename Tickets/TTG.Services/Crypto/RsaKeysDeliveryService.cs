using System.Threading;
using System.Threading.Tasks;
using Moonshine.Core.Security.Cryptography.Contracts;
using Moonshine.Core.Security.Cryptography.KeyStore.Contracts;
using Moonshine.Services.Crypto.Contracts;

namespace Moonshine.Services.Crypto;

public class RsaKeysDeliveryService : IKeysDeliveryService
{
    private readonly IAsymmetricKeyStoreProvider _keyStoreProvider;

    public RsaKeysDeliveryService(IAsymmetricKeyStoreProvider keyStoreProvider)
    {
        _keyStoreProvider = keyStoreProvider;
    }

    public async Task<string> GetPublicKey(CancellationToken ct)
    {
        var keys = await _keyStoreProvider.GetKeyInfo(ct);
        return keys.PublicKey;
    }
}