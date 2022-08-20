using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moonshine.Core.Caching;
using Moonshine.Core.Extensions;
using Moonshine.Core.Security.Cryptography.Contracts;
using Moonshine.Core.Security.Cryptography.Enums;
using Moonshine.Core.Security.Cryptography.KeyStore.Contracts;
using Moonshine.Core.Security.Cryptography.KeyStore.Dto;

namespace Moonshine.Core.Security.Cryptography.Rsa;

public class RsaCryptoService : ICryptoService, ISignatureService
{
    private readonly AsyncLazy<AsymmetricKeyStoreValue> _keysProvider;
    public RsaCryptoService(IAsymmetricKeyStoreProvider keyStoreProvider)
    {
        _keysProvider = new AsyncLazy<AsymmetricKeyStoreValue>(async ct 
            => await keyStoreProvider.GetKeyInfo(ct));
    }

    public async ValueTask<string> Decrypt(byte[] text, CancellationToken ct)
    {
        using var rsaProvider = await GetRsaCryptoServiceProvider(KeyType.Private, ct);
        var plainBytes = rsaProvider.Decrypt(text, true);
        return GetTextFromBytes(plainBytes);
    }

    public async ValueTask<byte[]> Encrypt(string text, CancellationToken ct)
    {
        using var rsaProvider = await GetRsaCryptoServiceProvider(KeyType.Public, ct);
        var plainBytes = GetTextBytes(text);
        var encryptedBytes = rsaProvider.Encrypt(plainBytes, true);
        return encryptedBytes;
    }
    
    public async ValueTask<byte[]> Sign(string text, CancellationToken ct)
    {
        using var rsaProvider = await GetRsaCryptoServiceProvider(KeyType.Private, ct);
        var textBytes = GetTextBytes(text);
        var signature = rsaProvider.SignData(textBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return signature;
    }

    public ValueTask<byte[]> Sign<T>(T obj, CancellationToken ct)
    {
        return Sign(obj.ToJson(), ct);
    }

    public async ValueTask<bool> Verify(byte[] text, byte[] signature, CancellationToken ct)
    {
        using var rsaProvider = await GetRsaCryptoServiceProvider(KeyType.Public, ct);
        var result = rsaProvider.VerifyData(text, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return result;
    }

    public ValueTask<bool> Verify(string text, byte[] signature, CancellationToken ct)
    {
        var bytes = GetTextBytes(text);
        return Verify(bytes, signature, ct);
    }

    public ValueTask<bool> Verify<T>(T obj, byte[] signature, CancellationToken ct)
    {
        var bytes = GetTextBytes(obj.ToJson());
        return Verify(bytes, signature, ct);
    }

    private static byte[] GetTextBytes(string text) => Encoding.UTF8.GetBytes(text);

    private static string GetTextFromBytes(byte[] bytes) => Encoding.UTF8.GetString(bytes);

    private async ValueTask<RSACryptoServiceProvider> GetRsaCryptoServiceProvider(KeyType keyType, CancellationToken ct)
    {
        var keys = await _keysProvider.GetValue(ct);
        var rsaProvider = new RSACryptoServiceProvider();
        
        /*
        Be careful with RSA keys format
        “BEGIN RSA PRIVATE KEY” => RSA.ImportRSAPrivateKey
        “BEGIN PRIVATE KEY” => RSA.ImportPkcs8PrivateKey
        “BEGIN ENCRYPTED PRIVATE KEY” => RSA.ImportEncryptedPkcs8PrivateKey
        “BEGIN RSA PUBLIC KEY” => RSA.ImportRSAPublicKey
        “BEGIN PUBLIC KEY” => RSA.ImportSubjectPublicKeyInfo
        https://vcsjones.dev/key-formats-dotnet-3/
        */
        
        switch (keyType)
        {
            case KeyType.Public:
                rsaProvider.ImportSubjectPublicKeyInfo(Convert.FromBase64String(keys.PublicKey), out _);
                break;
            case KeyType.Private:
                rsaProvider.ImportRSAPrivateKey(Convert.FromBase64String(keys.PrivateKey), out _);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(keyType), keyType, null);
        }
        
        return rsaProvider;
    }
}