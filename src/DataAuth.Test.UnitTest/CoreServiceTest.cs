using DataAuth.Core;
using DataAuth.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DataAuth.Test.UnitTest
{
    [TestClass]
    public class CoreServiceTest
    {
        ICoreService _coreService;

        public CoreServiceTest()
        {
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: "DataAuthTest")
                .Options;
            var dbContext = new DataAuthDbContext(options);
            _coreService = new CoreService(dbContext);
        }

        //[TestMethod]
        //public async Task CanAccessTestDatabase()
        //{
        //    var data = await _coreService.GetDataPermissions("1", DataAuth.Enums.GrantType.ForRole);
        //    Assert.IsNotNull(data);
        //}

        [TestMethod]
        public void GenerateQuery_GlobalLevel()
        {
            var permission = new DataPermission(Enums.GrantType.ForUser, "1", 1, Enums.AccessLevel.Global);
            permission.AccessAttributeTable = new AccessAttributeTable(1, "Stores", "s", "Id", "Name");
            var query = _coreService.GenerateQueryString(permission);
            Assert.AreEqual("SELECT s.[Id] FROM [Stores] AS s", query.QueryString);
        }

        [TestMethod]
        public void GenerateQuery_LocalLevel()
        {
            var permission = new DataPermission(Enums.GrantType.ForUser, "1", 1, Enums.AccessLevel.Local);
            permission.AccessAttributeTable = new AccessAttributeTable(1, "Stores", "s", "Id", "Name", null, 1, "Employees", "Id", "LastWorkingStoreId");
            var query = _coreService.GenerateQueryString(permission, "100");
            Assert.AreEqual("SELECT s.[Id] FROM [Stores] AS s\nWHERE s.[Id] IN (SELECT [LastWorkingStoreId] FROM [Employees] WHERE [Id] = @LookupValue)", query.QueryString);
            Assert.IsNotNull(query.QueryParams);
            Assert.AreEqual(1, query.QueryParams.Count);
            Assert.AreEqual("LookupValue", query.QueryParams[0].ParameterName);
            Assert.AreEqual("100", query.QueryParams[0].Value);
        }
    }
}