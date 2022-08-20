using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moonshine.Core.Security.Cryptography.Contracts;
using Xunit;

namespace MoonshineTest.Services.CryptoTests;

public class CryptoServiceTests : BaseCryptoTest
{
    private readonly ICryptoService _cryptoService;

    public CryptoServiceTest()
    {
        _cryptoService = ServiceProvider.GetRequiredService<ICryptoService>();
    }

    [Theory]
    [InlineData( "test text to test encrypt and decrypt")]
    public async Task Encrypt_And_Decrypt_Success(string testText)
    {
        var textAfterEncryption = await _cryptoService.Encrypt(testText, default);
        Assert.NotEmpty(textAfterEncryption);
        
        var textAfterDecryption = await _cryptoService.Decrypt(textAfterEncryption, default);
        Assert.NotEmpty(textAfterDecryption);
        Assert.Equal(textAfterDecryption, testText);
    }
    
    [Theory]
    [InlineData( "text to alter")]
    public async Task Encrypt_And_Decrypt_Fails(string alterText)
    {
        var alteredBytes = Encoding.UTF8.GetBytes(alterText);
        var task = _cryptoService.Decrypt(alteredBytes, default);
        await Assert.ThrowsAnyAsync<Exception>(async () => await task);
    }
}