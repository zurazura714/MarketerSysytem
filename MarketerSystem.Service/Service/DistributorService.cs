using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.Policies;
using Microsoft.EntityFrameworkCore;

namespace MarketerSystem.Service.Service;

public class DistributorService(
    IUnitOfWork context,
    IDistributorRepository distributorRepository,
    IAddressRepository addressRepository,
    IContactInfoRepository contactInfoRepository,
    IPictureRepository pictureRepository)
    : ServiceBase<Distributor, IDistributorRepository>(context, distributorRepository), IDistributorService
{
    public Task<List<Distributor>> ListWithDetailsAsync() => DetailedQuery().ToListAsync();

    public Task<Distributor?> FetchWithDetailsAsync(int id) =>
        DetailedQuery().FirstOrDefaultAsync(d => d.DistributorID == id);

    private IQueryable<Distributor> DetailedQuery() =>
        _repository.Set()
            .Include(d => d.Passport)
            .Include(d => d.Pictures)
            .Include(d => d.ContactInfos)
            .Include(d => d.Addresses)
            .AsSplitQuery();

    public async Task<Distributor> CreateAsync(Distributor distributor)
    {
        ArgumentNullException.ThrowIfNull(distributor);

        distributor.DistributorGuid = Guid.NewGuid();
        distributor.GenerationLinker = await BuildGenerationLinkerAsync(distributor.RecomendatorID);

        await SaveAsync(distributor);
        return distributor;
    }

    private async Task<string?> BuildGenerationLinkerAsync(int? recomendatorId)
    {
        if (recomendatorId is not int id || id == 0)
        {
            return null;
        }

        var recomendator = await _repository.FetchAsync(id)
            ?? throw new ArgumentException($"Recomendator {id} does not exist", nameof(recomendatorId));

        if (GenerationChain.IsAtMaxDepth(recomendator.GenerationLinker))
        {
            throw new ArgumentException("Try Other Recomendator, Its limit has been reached", nameof(recomendatorId));
        }

        return GenerationChain.Append(recomendator.GenerationLinker, recomendator.DistributorID);
    }

    public async Task<Distributor?> UpdateAsync(int id, Distributor updated)
    {
        ArgumentNullException.ThrowIfNull(updated);

        var existing = await FetchWithDetailsAsync(id);
        if (existing == null)
        {
            return null;
        }

        existing.FirstName = updated.FirstName;
        existing.LastName = updated.LastName;
        existing.BirthDate = updated.BirthDate;
        existing.Gender = updated.Gender;

        if (updated.Addresses is { Count: > 0 })
        {
            foreach (var old in existing.Addresses.ToList())
            {
                await addressRepository.DeleteAsync(old);
            }
            existing.Addresses.Clear();
            foreach (var address in updated.Addresses)
            {
                address.DistributorID = existing.DistributorID;
                existing.Addresses.Add(address);
            }
        }

        if (updated.ContactInfos is { Count: > 0 })
        {
            foreach (var old in existing.ContactInfos.ToList())
            {
                await contactInfoRepository.DeleteAsync(old);
            }
            existing.ContactInfos.Clear();
            foreach (var contactInfo in updated.ContactInfos)
            {
                contactInfo.DistributorID = existing.DistributorID;
                existing.ContactInfos.Add(contactInfo);
            }
        }

        if (updated.Pictures is { Count: > 0 })
        {
            existing.Pictures ??= [];
            foreach (var old in existing.Pictures.ToList())
            {
                await pictureRepository.DeleteAsync(old);
            }
            existing.Pictures.Clear();
            foreach (var picture in updated.Pictures)
            {
                picture.DistributorID = existing.DistributorID;
                existing.Pictures.Add(picture);
            }
        }

        // Single commit — scalar changes and all child replacements land atomically.
        await _context.CommitAsync();
        return existing;
    }
}
