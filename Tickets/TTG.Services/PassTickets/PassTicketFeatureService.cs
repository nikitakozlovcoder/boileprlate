using System.Threading;
using System.Threading.Tasks;
using Moonshine.Core.Features.Contracts;
using Moonshine.Data.Repositories.Interfaces;
using Moonshine.Services.PassTickets.Contracts;

namespace Moonshine.Services.PassTickets;

public class PassTicketFeatureService : IPassTicketFeatureService
{
    private readonly IProductRepository _productRepository;

    public PassTicketFeatureService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public ValueTask<bool> IsFeatureEnabled(IHasPassTicketFeature featureHolder)
    {
        return new ValueTask<bool>(featureHolder.UsePassTicket);
    }

    public Task<bool> IsFeatureEnabled(int id, CancellationToken ct)
    {
        return _productRepository.AnyAsync(x => x.Id == id && x.UsePassTicket, ct);
    }
}