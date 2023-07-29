using DataAuth;
using DataAuth.Domains.AccessAttributes;
using DataAuth.Domains.AccessAttributeTables;
using DataAuth.Cache;
using DataAuth.Core;
using DataAuth.Domains.DataPermissions;
using DataAuth.Domains.Roles;
using DataAuth.Sample.WebApi;
using DataAuth.Domains.UserRoles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer("name=ConnectionStrings:ApplicationDbContext")
);
builder.Services.AddDbContext<DataAuthDbContext>(
    options =>
        options.UseSqlServer(
            "name=ConnectionStrings:ApplicationDbContext",
            b => b.MigrationsAssembly("DataAuth.Sample.WebApi")
        )
);
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
builder.Services.AddScoped<ICoreService, CoreService>();
builder.Services.AddScoped<IAccessAttributeService, AccessAttributeService>();
builder.Services.AddScoped<IAccessAttributeTableService, AccessAttributeTableService>();
builder.Services.AddScoped<IDataPermissionService, DataPermissionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

var app = builder.Build();

EfExtensions.Initialize(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
