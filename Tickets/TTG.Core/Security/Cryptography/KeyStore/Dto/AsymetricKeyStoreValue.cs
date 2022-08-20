namespace Moonshine.Core.Security.Cryptography.KeyStore.Dto;

public record AsymmetricKeyStoreValue
{
    public string PublicKey { get; init; }
    public string PrivateKey { get; init; }
}