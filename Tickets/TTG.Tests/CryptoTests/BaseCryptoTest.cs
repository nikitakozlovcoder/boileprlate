using System.Threading;
using Moonshine.Core.Security.Cryptography.KeyStore.Contracts;
using Moonshine.Core.Security.Cryptography.KeyStore.Dto;
using MoonshineTest.Moqs;
using Moq;

namespace MoonshineTest.Services.CryptoTests;

public class BaseCryptoTest : BaseTest
{
    protected BaseCryptoTest()
    {
        AddMoq<IAsymmetricKeyStoreProvider>(moq =>
        {
            moq.Setup(x => x.GetKeyInfo(It.IsAny<CancellationToken>())).ReturnsAsync(new AsymmetricKeyStoreValue
            {
                PrivateKey = AsymmetricKeyStoreValuesMoq.PrivateKey,
                PublicKey = AsymmetricKeyStoreValuesMoq.PublicKey
            });
        });
    }
}