using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moonshine.Core.Security.Cryptography.Contracts;
using Xunit;

namespace MoonshineTest.Services.CryptoTests;

public class SignatureServiceTests : BaseCryptoTest
{
    private readonly ISignatureService _signatureService;

    public SignatureServiceTest()
    {
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