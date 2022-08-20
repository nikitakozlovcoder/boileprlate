using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moonshine.Core.Configuration.Crypto;
using Moonshine.Core.Security.Cryptography.KeyStore.Contracts;
using Moonshine.Core.Security.Cryptography.KeyStore.Dto;

namespace Moonshine.Core.Security.Cryptography.KeyStore;

public class LocalAsymmetricFileKeyStoreProvider : IAsymmetricKeyStoreProvider
{
    private readonly RsaCryptoOptions _settings;

    public LocalAsymmetricFileKeyStoreProvider(IOptions<RsaCryptoOptions> settings)
    {
        _settings = settings.Value;
    }

    public async ValueTask<AsymmetricKeyStoreValue> GetKeyInfo(CancellationToken _)
    {
        using var publicKeyReader = new StreamReader(_settings.PublicKeyPath);
        using var privateReader = new StreamReader(_settings.PrivateKeyPath);
        var publicKey = await publicKeyReader.ReadToEndAsync();
        var privateKey = await privateReader.ReadToEndAsync();
        var keys = new AsymmetricKeyStoreValue
        {
            PublicKey = publicKey.Trim(),
            PrivateKey = privateKey.Trim()
        };

        return keys;
    }
}