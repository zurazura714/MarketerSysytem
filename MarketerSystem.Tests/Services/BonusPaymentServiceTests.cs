using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;
using MarketerSystem.Service.Service;
using Moq;
using Shouldly;

namespace MarketerSystem.Tests.Services;

public class BonusPaymentServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IBonusPaymentRepository> _repo = new();
    private readonly BonusPaymentService _sut;

    public BonusPaymentServiceTests()
    {
        _sut = new BonusPaymentService(_uow.Object, _repo.Object);
    }

    [Fact]
    public async Task FilterPaymentsProducts_NullParameters_ThrowsArgumentNullException()
    {
        await Should.ThrowAsync<ArgumentNullException>(() => _sut.FilterPaymentsProducts(null!));
    }

    [Fact]
    public async Task FilterPaymentsProducts_NoFilters_ReturnsAllPayments()
    {
        _repo.Setup(r => r.SetAsync()).ReturnsAsync(SamplePayments());

        var result = await _sut.FilterPaymentsProducts(new PaymentFilterParameters());

        result.Count.ShouldBe(3);
    }

    [Fact]
    public async Task FilterPaymentsProducts_MinPriceFilter_ExcludesLowerBonuses()
    {
        _repo.Setup(r => r.SetAsync()).ReturnsAsync(SamplePayments());

        var result = await _sut.FilterPaymentsProducts(new PaymentFilterParameters { MinPrice = 50 });

        result.Count.ShouldBe(2);
        result.ShouldAllBe(p => p.BonusPay >= 50);
    }

    [Fact]
    public async Task FilterPaymentsProducts_MaxPriceFilter_ExcludesHigherBonuses()
    {
        _repo.Setup(r => r.SetAsync()).ReturnsAsync(SamplePayments());

        var result = await _sut.FilterPaymentsProducts(new PaymentFilterParameters { MaxPrice = 50 });

        result.Count.ShouldBe(2);
        result.ShouldAllBe(p => p.BonusPay <= 50);
    }

    [Fact]
    public async Task FilterPaymentsProducts_NameFilter_MatchesDistributorFirstName()
    {
        _repo.Setup(r => r.SetAsync()).ReturnsAsync(SamplePayments());

        var result = await _sut.FilterPaymentsProducts(new PaymentFilterParameters { Name = "Zura" });

        var only = result.ShouldHaveSingleItem();
        only.Distributor.FirstName.ShouldBe("Zura");
    }

    [Fact]
    public async Task FilterPaymentsProducts_LastNameFilter_MatchesDistributorLastName()
    {
        _repo.Setup(r => r.SetAsync()).ReturnsAsync(SamplePayments());

        var result = await _sut.FilterPaymentsProducts(new PaymentFilterParameters { LastName = "Beridze" });

        var only = result.ShouldHaveSingleItem();
        only.Distributor.LastName.ShouldBe("Beridze");
    }

    [Fact]
    public async Task SaveAsync_PersistsThroughRepositoryAndCommitsUnit()
    {
        var payment = new BonusPayment { ID = 99, BonusPay = 25, DistributorID = 1 };

        await _sut.SaveAsync(payment);

        _repo.Verify(r => r.SaveAsync(payment), Times.Once);
        _uow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityExists_DeletesAndCommits()
    {
        var payment = new BonusPayment { ID = 1, BonusPay = 10 };
        _repo.Setup(r => r.FetchAsync(1)).ReturnsAsync(payment);

        await _sut.DeleteAsync(1);

        _repo.Verify(r => r.DeleteAsync(payment), Times.Once);
        _uow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityMissing_DoesNotCommit()
    {
        _repo.Setup(r => r.FetchAsync(404)).ReturnsAsync((BonusPayment)null!);

        await _sut.DeleteAsync(404);

        _repo.Verify(r => r.DeleteAsync(It.IsAny<BonusPayment>()), Times.Never);
        _uow.Verify(u => u.CommitAsync(), Times.Never);
    }

    private static List<BonusPayment> SamplePayments() =>
        new()
        {
            new BonusPayment
            {
                ID = 1, BonusPay = 25, DistributorID = 1,
                Distributor = new Distributor { DistributorID = 1, FirstName = "Zura", LastName = "Samkharadze" }
            },
            new BonusPayment
            {
                ID = 2, BonusPay = 50, DistributorID = 2,
                Distributor = new Distributor { DistributorID = 2, FirstName = "Maiko", LastName = "Samkharadze" }
            },
            new BonusPayment
            {
                ID = 3, BonusPay = 100, DistributorID = 3,
                Distributor = new Distributor { DistributorID = 3, FirstName = "Nika", LastName = "Beridze" }
            },
        };
}
