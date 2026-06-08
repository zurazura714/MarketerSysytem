using Mapster;
using MapsterMapper;
using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Data.Context;
using MarketerSystem.Repository.Repository;
using MarketerSystem.Service.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<MarketerDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("MarketerDBContext")));

AddMapster(builder.Services);
AddRepositoriesAndServices(builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.MapScalarApiReference();
}

UpdateDatabase(app);

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();


static void UpdateDatabase(IApplicationBuilder app)
{
    using var serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();
    var context = serviceScope.ServiceProvider.GetRequiredService<MarketerDBContext>();
    context.Database.Migrate();
}

static void AddMapster(IServiceCollection services)
{
    var mapsterConfig = TypeAdapterConfig.GlobalSettings;
    mapsterConfig.Scan(typeof(Program).Assembly);
    services.AddSingleton(mapsterConfig);
    services.AddScoped<IMapper, ServiceMapper>();
}

static void AddRepositoriesAndServices(IServiceCollection services)
{
    services.AddScoped<IUnitOfWork, MarketerDBContext>();

    services.AddScoped<IDistributorRepository, DistributorRepository>();
    services.AddScoped<IPictureRepository, PictureRepository>();
    services.AddScoped<IContactInfoRepository, ContactInfoRepository>();
    services.AddScoped<IAddressRepository, AddressRepository>();
    services.AddScoped<IPassportRepository, PassportRepository>();
    services.AddScoped<IProductRepository, ProductRepository>();
    services.AddScoped<ISellRepository, SellRepository>();
    services.AddScoped<IBonusPaymentRepository, BonusPaymentRepository>();

    services.AddScoped<IDistributorService, DistributorService>();
    services.AddScoped<IPictureService, PictureService>();
    services.AddScoped<IContactInfoService, ContactInfoService>();
    services.AddScoped<IAddressService, AddressService>();
    services.AddScoped<IPassportService, PassportService>();
    services.AddScoped<IProductService, ProductService>();
    services.AddScoped<ISellService, SellService>();
    services.AddScoped<IBonusPaymentService, BonusPaymentService>();
}
