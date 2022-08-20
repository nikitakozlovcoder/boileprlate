using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moonshine.Core.Security.Cryptography.Contracts;
using Moonshine.Core.Security.Cryptography.KeyStore.Contracts;
using Moonshine.Core.Security.Cryptography.KeyStore.Dto;
using MoonshineTest.Moqs;
using Moq;
using Xunit;

namespace MoonshineTest.Services.CryptoTests;

public class SignatureServiceTests : BaseTest
{
    private readonly ISignatureService _signatureService;

    public SignatureServiceTest()
    {
        AddMoq<IAsymmetricKeyStoreProvider>(moq =>
        {
            moq.Setup(x => x.GetKeyInfo(It.IsAny<CancellationToken>())).ReturnsAsync(new AsymmetricKeyStoreValue
            {
                PrivateKey = AsymmetricKeyStoreValuesMoq.PrivateKey,
                PublicKey = AsymmetricKeyStoreValuesMoq.PublicKey
            });
        });
        
        _signatureService = ServiceProvider.GetRequiredService<ISignatureService>();

    }

    [Theory]
    [InlineData( "test text to test signature")]
    public async Task Sign_And_Verify_Success(string testText)
    {
        var signature = await _signatureService.Sign(testText, default);
        Assert.NotEmpty(signature);
        
        var result = await _signatureService.Verify(testText, signature, default);
        Assert.True(result);
    }
    
    [Theory]
    [InlineData( "test text to test signature", "altered text")]
    public async Task Sign_And_Verify_Fails(string testText, string alteredText)
    {
        var signature = await _signatureService.Sign(testText, default);
        Assert.NotEmpty(signature);
        
        var result = await _signatureService.Verify(alteredText, signature, default);
        Assert.False(result);
    }
}