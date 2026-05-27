using MarketerSystem.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MarketerSystem.Tests.Infrastructure;

internal static class InMemoryContextFactory
{
    public static MarketerDBContext Create()
    {
        var options = new DbContextOptionsBuilder<MarketerDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .EnableSensitiveDataLogging()
            .Options;

        return new MarketerDBContext(options);
    }
}
