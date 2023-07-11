# Introduction

This project aims to create a mechanism for data authorization.
...

# Getting started

1. Adding nuget package ...
2. In your main application Program.cs, adding dependency injection for services
   ```csharp
   builder.Services.AddDbContext<DataAuthDbContext>(options => options.UseSqlServer("name=ConnectionStrings:YourConnectionStringName",
   b => b.MigrationsAssembly("YourAssemblyName")));
   builder.Services.AddScoped<ICoreService, CoreService>();
   builder.Services.AddScoped<IAccessAttributeService, AccessAttributeService>();
   builder.Services.AddScoped<IAccessAttributeTableService, AccessAttributeTableService>();
   builder.Services.AddScoped<IDataPermissionService, DataPermissionService>();
   builder.Services.AddScoped<IRoleService, RoleService>();
   builder.Services.AddScoped<IUserRoleService, UserRoleService>();
   builder.Services.AddScoped<ICoreService, CoreService>();
   ```
3. Also in your main application Program.cs, below `var app = builder.Build();`, add the following line:
   ```csharp
   EfExtensions.Initialize(app.Services);
   ```
4. In your data access service, using `WithDataAuthAsync` extension method to filter data by data permission. For example:

   ```csharp
   using DataAuth.Core;

   var orders = await _dbContext.Orders.WithDataAuthAsync<Order, int>("2", "STORE", x => x.StoreId.Value);
   ```
