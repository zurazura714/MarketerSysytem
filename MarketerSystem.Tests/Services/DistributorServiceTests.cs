using MarketerSystem.Common.Enums;
using MarketerSystem.Data.Context;
using MarketerSystem.Domain.Model;
using MarketerSystem.Repository.Repository;
using MarketerSystem.Service.Service;
using MarketerSystem.Tests.Infrastructure;
using Shouldly;

namespace MarketerSystem.Tests.Services;

public sealed class DistributorServiceTests : IDisposable
{
    private readonly MarketerDBContext _db;
    private readonly DistributorService _sut;

    public DistributorServiceTests()
    {
        _db = InMemoryContextFactory.Create();
        _sut = new DistributorService(
            _db,
            new DistributorRepository(_db),
            new AddressRepository(_db),
            new ContactInfoRepository(_db),
            new PictureRepository(_db));
    }

    public void Dispose() => _db.Dispose();

    private static Distributor NewDistributor(int id = 0, string? linker = null, int? recomendatorId = null) => new()
    {
        DistributorID = id,
        FirstName = "Test",
        LastName = "Distributor",
        BirthDate = new DateTime(1990, 1, 1),
        GenerationLinker = linker,
        RecomendatorID = recomendatorId
    };

    private async Task SeedAsync(Distributor distributor)
    {
        _db.Distributors.Add(distributor);
        await _db.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateAsync_NullDistributor_Throws()
    {
        await Should.ThrowAsync<ArgumentNullException>(() => _sut.CreateAsync(null!));
    }

    [Fact]
    public async Task CreateAsync_NoRecomendator_PersistsWithGuidAndNoLinker()
    {
        var created = await _sut.CreateAsync(NewDistributor());

        created.DistributorGuid.ShouldNotBe(Guid.Empty);
        created.GenerationLinker.ShouldBeNull();
        _db.Distributors.Count().ShouldBe(1);
    }

    [Fact]
    public async Task CreateAsync_RecomendatorZero_TreatedAsNone()
    {
        var created = await _sut.CreateAsync(NewDistributor(recomendatorId: 0));

        created.GenerationLinker.ShouldBeNull();
    }

    [Fact]
    public async Task CreateAsync_RootRecomendator_LinkerIsRecomendatorId()
    {
        await SeedAsync(NewDistributor(id: 1));

        var created = await _sut.CreateAsync(NewDistributor(recomendatorId: 1));

        created.GenerationLinker.ShouldBe("1");
    }

    [Fact]
    public async Task CreateAsync_ChainedRecomendator_AppendsToParentChain()
    {
        await SeedAsync(NewDistributor(id: 3, linker: "1,2"));

        var created = await _sut.CreateAsync(NewDistributor(recomendatorId: 3));

        created.GenerationLinker.ShouldBe("1,2,3");
    }

    [Fact]
    public async Task CreateAsync_MissingRecomendator_ThrowsArgumentException()
    {
        var ex = await Should.ThrowAsync<ArgumentException>(() => _sut.CreateAsync(NewDistributor(recomendatorId: 999)));
        ex.Message.ShouldContain("999");
    }

    [Fact]
    public async Task CreateAsync_RecomendatorAtMaxDepth_ThrowsArgumentException()
    {
        await SeedAsync(NewDistributor(id: 6, linker: "1,2,3,4,5"));

        await Should.ThrowAsync<ArgumentException>(() => _sut.CreateAsync(NewDistributor(recomendatorId: 6)));
    }

    [Fact]
    public async Task UpdateAsync_MissingDistributor_ReturnsNull()
    {
        var result = await _sut.UpdateAsync(404, NewDistributor());

        result.ShouldBeNull();
    }

    [Fact]
    public async Task UpdateAsync_UpdatesScalarFields()
    {
        await SeedAsync(NewDistributor(id: 1));
        var updated = NewDistributor();
        updated.FirstName = "Renamed";
        updated.LastName = "Person";

        var result = await _sut.UpdateAsync(1, updated);

        result.ShouldNotBeNull();
        _db.Distributors.Single(d => d.DistributorID == 1).FirstName.ShouldBe("Renamed");
    }

    [Fact]
    public async Task UpdateAsync_ReplacesChildren_RepeatedPutDoesNotDuplicate()
    {
        var existing = NewDistributor(id: 1);
        existing.Addresses = [new Address { AddressType = AddressType.Actual, AddressInfo = "Old street", DistributorID = 1 }];
        await SeedAsync(existing);

        Distributor UpdatePayload() => new()
        {
            FirstName = "Test",
            LastName = "Distributor",
            BirthDate = new DateTime(1990, 1, 1),
            Addresses =
            [
                new Address { AddressType = AddressType.Actual, AddressInfo = "New street 1" },
                new Address { AddressType = AddressType.Registration, AddressInfo = "New street 2" }
            ]
        };

        await _sut.UpdateAsync(1, UpdatePayload());
        await _sut.UpdateAsync(1, UpdatePayload());

        var addresses = _db.Addresses.Where(a => a.DistributorID == 1).ToList();
        addresses.Count.ShouldBe(2);
        addresses.ShouldAllBe(a => a.AddressInfo.StartsWith("New street"));
    }

    [Fact]
    public async Task UpdateAsync_NoChildrenProvided_LeavesExistingChildrenUntouched()
    {
        var existing = NewDistributor(id: 1);
        existing.ContactInfos = [new ContactInfo { ContactInformationType = ContactInformationType.Mobile, Information = "599000000", DistributorID = 1 }];
        await SeedAsync(existing);

        await _sut.UpdateAsync(1, NewDistributor());

        _db.Set<ContactInfo>().Count(c => c.DistributorID == 1).ShouldBe(1);
    }

    [Fact]
    public async Task FetchWithDetailsAsync_LoadsChildCollections()
    {
        var existing = NewDistributor(id: 1);
        existing.Addresses = [new Address { AddressType = AddressType.Actual, AddressInfo = "Street", DistributorID = 1 }];
        existing.ContactInfos = [new ContactInfo { ContactInformationType = ContactInformationType.Email, Information = "a@b.c", DistributorID = 1 }];
        await SeedAsync(existing);

        var result = await _sut.FetchWithDetailsAsync(1);

        result.ShouldNotBeNull();
        result.Addresses.ShouldHaveSingleItem();
        result.ContactInfos.ShouldHaveSingleItem();
    }

    [Fact]
    public async Task ListWithDetailsAsync_ReturnsAllDistributors()
    {
        await SeedAsync(NewDistributor(id: 1));
        await SeedAsync(NewDistributor(id: 2));

        var result = await _sut.ListWithDetailsAsync();

        result.Count.ShouldBe(2);
    }
}
