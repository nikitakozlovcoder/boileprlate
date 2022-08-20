using System;
using System.Threading;
using System.Threading.Tasks;
using Moonshine.Core.Extensions;
using Moonshine.Core.Security.Cryptography.Contracts;
using Moonshine.Data.Repositories.Interfaces;
using Moonshine.Services.PassTickets.Contracts;
using Moonshine.Services.PassTickets.Dto;
using Moonshine.Services.QrCode;

namespace Moonshine.Services.PassTickets;

public class QrCodePassTicketService : IPassTicketService
{
    private readonly IQrCodeHelper _qrCodeHelper;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly ISignatureService _signatureService;

    public QrCodePassTicketService(
        IQrCodeHelper qrCodeHelper,
        IOrderItemRepository orderItemRepository,
        ISignatureService signatureService)
    {
        _qrCodeHelper = qrCodeHelper;
        _orderItemRepository = orderItemRepository;
        _signatureService = signatureService;
    }

    public async Task<byte[]> GenerateTicketInfo(int orderItemId, CancellationToken ct)
    {
        var passJson = await GetPassJson(orderItemId, ct);
        await _qrCodeHelper.GenerateQrCodeAsync(passJson);
        var qrCode = _qrCodeHelper.QrInByteArray;
        return qrCode;
    }
    
    public async Task<string> GenerateTicketInfoBase64(int orderItemId, CancellationToken ct)
    {
        var passJson = await GetPassJson(orderItemId, ct);
        await _qrCodeHelper.GenerateQrCodeAsync(passJson);
        var qrCode = _qrCodeHelper.QrInBase64;
        return qrCode;
    }
    
    public async Task<bool> CheckValidity(SignedPassTicketDto ticketDto, CancellationToken ct)
    {
        var signatureBytes = Convert.FromBase64String(ticketDto.Base64Signature);
        var result = await _signatureService.Verify(ticketDto.Payload, signatureBytes, ct);
        return result;
    }

    private async Task<string> GetPassJson(int orderItemId, CancellationToken ct)
    {
        var passTicketPayloadDto = await _orderItemRepository.ProjectTo<PassTicketDto>(x => x.Id == orderItemId, ct);
        var signature = await _signatureService.Sign(passTicketPayloadDto, ct);
        
        var passDto = new SignedPassTicketDto
        {
            Payload = passTicketPayloadDto,
            Base64Signature = Convert.ToBase64String(signature)
        };

        var passJson = passDto.ToJson();
        return passJson;
    }
}