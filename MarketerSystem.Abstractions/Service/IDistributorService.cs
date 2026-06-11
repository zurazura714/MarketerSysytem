using MarketerSystem.Domain.Model;

namespace MarketerSystem.Abstractions.Service;

public interface IDistributorService : IServiceBase<Distributor>
{
    Task<List<Distributor>> ListWithDetailsAsync();
    Task<Distributor?> FetchWithDetailsAsync(int id);

    /// <summary>Assigns the Guid, builds the GenerationLinker from the recomendator, persists.</summary>
    Task<Distributor> CreateAsync(Distributor distributor);

    /// <summary>
    /// Updates scalar fields and replaces any provided child collections atomically.
    /// Returns null when the distributor does not exist.
    /// </summary>
    Task<Distributor?> UpdateAsync(int id, Distributor updated);
}
