using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Data.Context;
using MarketerSystem.Repository.Repository;
using MarketerSystem.Service.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MarketerDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("MarketerDBContext")));

AddAuth(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
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
    using (var serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope())
    {
        var context = serviceScope.ServiceProvider.GetService<MarketerDBContext>();
        context.Database.Migrate();
    }
}

static void AddAuth(IServiceCollection services)
{
    services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
    options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

    // authentication 
    services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    });

}


static void AddRepositoriesAndServices(IServiceCollection services)
{
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    services.AddScoped<IUnitOfWork, MarketerDBContext>();

    services.AddScoped<IDistributorRepository, DistributorRepository>();
    //services.AddScoped<ITaskFileRepository, TaskFileRepository>();
    //services.AddScoped<IUserRepository, UserRepository>();
    //services.AddScoped<IUserRoleRepository, UserRoleRepository>();

    services.AddScoped<IDistributorService, DistributorService>();
    //services.AddScoped<ITaskFileService, TaskFileService>();
    //services.AddScoped<IUserService, UserService>();
    //services.AddScoped<IUserRoleService, UserRoleService>();
}