using System.Threading;
using System.Threading.Tasks;
using Moonshine.Services.PassTickets.Dto;

namespace Moonshine.Services.PassTickets.Contracts;

public interface IPassTicketService
{
    Task<byte[]> GenerateTicketInfo(int orderItemId, CancellationToken ct);
    Task<string> GenerateTicketInfoBase64(int orderItemId, CancellationToken ct);
    Task<bool> CheckValidity(SignedPassTicketDto ticketDto, CancellationToken ct);
}