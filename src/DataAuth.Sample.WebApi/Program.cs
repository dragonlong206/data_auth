using DataAuth;
using DataAuth.AccessAttributes;
using DataAuth.AccessAttributeTables;
using DataAuth.Core;
using DataAuth.DataPermissions;
using DataAuth.Roles;
using DataAuth.Sample.WebApi;
using DataAuth.UserRoles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("name=ConnectionStrings:ApplicationDbContext"));
builder.Services.AddDbContext<DataAuthDbContext>(options => options.UseSqlServer("name=ConnectionStrings:ApplicationDbContext",
    b => b.MigrationsAssembly("DataAuth.Sample.WebApi")));
builder.Services.AddScoped<ICoreService, CoreService>();
builder.Services.AddScoped<IAccessAttributeService, AccessAttributeService>();
builder.Services.AddScoped<IAccessAttributeTableService, AccessAttributeTableService>();
builder.Services.AddScoped<IDataPermissionService, DataPermissionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<ICoreService, CoreService>();

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
