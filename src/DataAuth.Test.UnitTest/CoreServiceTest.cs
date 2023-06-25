using DataAuth.Core;
using DataAuth.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
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

        [TestCategory("GenerateQuery")]
        [TestMethod]
        public async Task GlobalLevel()
        {
            var permission = new DataPermission(Enums.GrantType.ForUser, "1", 1, Enums.AccessLevel.Global);
            permission.AccessAttributeTable = new AccessAttributeTable(1, "Stores", "s", "Id", "Name");
            var query = await _coreService.GenerateQueryString(permission);
            Assert.AreEqual("SELECT s.[Id] FROM [Stores] AS s", query.QueryString);
        }

        [TestCategory("GenerateQuery")]
        [TestMethod]
        public async Task LocalLevel()
        {
            var permission = new DataPermission(Enums.GrantType.ForUser, "1", 1, Enums.AccessLevel.Local);
            permission.AccessAttributeTable = new AccessAttributeTable(1, "Stores", "s", "Id", "Name", null, 1, "Employees", "Id", "LastWorkingStoreId");
            var query = await _coreService.GenerateQueryString(permission, "100");
            Assert.AreEqual("SELECT s.[Id] FROM [Stores] AS s\nWHERE s.[Id] IN (SELECT [LastWorkingStoreId] FROM [Employees] WHERE [Id] = @LookupValue)", query.QueryString);
            Assert.IsNotNull(query.QueryParams);
            Assert.AreEqual(1, query.QueryParams.Count);
            Assert.AreEqual("LookupValue", query.QueryParams[0].ParameterName);
            Assert.AreEqual("100", query.QueryParams[0].Value);
        }

        [TestInitialize]

        [TestCategory("GenerateQuery")]
        [TestMethod]
        public async Task DeepLevel_LevelSeparatedHierarchy()
        {
            // Initialize

            // Exec
            //var permission = new DataPermission(Enums.GrantType.ForUser, "1", 1, Enums.AccessLevel.Local);
            //permission.AccessAttributeTable = new AccessAttributeTable(1, "Stores", "s", "Id", "Name", null, 1, "Employees", "Id", "LastWorkingStoreId");
            //var query = await _coreService.GenerateQueryString(permission, "100");

            // Cleanup

            //Assert.AreEqual("SELECT s.[Id] FROM [Stores] AS s\nWHERE s.[Id] IN (SELECT [LastWorkingStoreId] FROM [Employees] WHERE [Id] = @LookupValue)", query.QueryString);
            //Assert.IsNotNull(query.QueryParams);
            //Assert.AreEqual(1, query.QueryParams.Count);
            //Assert.AreEqual("LookupValue", query.QueryParams[0].ParameterName);
            //Assert.AreEqual("100", query.QueryParams[0].Value);
        }

        [TestCategory("CreateJoinQueryForHierarchy")]
        [TestMethod]
        public void NullLowerLevels()
        {
            var currentQuery = "CurrentQuery";
            var rootTable = new AccessAttributeTable(1, "Test", "t", "Id", "Name");
            var joinQuery = CoreService.CreateJoinQueryForHierarchy(currentQuery, rootTable, null);
            Assert.AreEqual(currentQuery, joinQuery);
        }

        [TestCategory("CreateJoinQueryForHierarchy")]
        [TestMethod]
        public void EmptyLowerLevels()
        {
            var currentQuery = "CurrentQuery";
            var rootTable = new AccessAttributeTable(1, "Test", "t", "Id", "Name");
            var joinQuery = CoreService.CreateJoinQueryForHierarchy(currentQuery, rootTable, new List<AccessAttributeTable>());
            Assert.AreEqual(currentQuery, joinQuery);
        }

        [TestCategory("CreateJoinQueryForHierarchy")]
        [TestMethod]
        public void OneLowerLevel()
        {
            var currentQuery = "SELECT r.[Id] FROM [Regions] AS r";
            var rootTable = new AccessAttributeTable(1, "Regions", "r", "Id", "Name", null, 1, null, null, null);
            var lowerLevelTables = new List<AccessAttributeTable>
            {
                new AccessAttributeTable(2, "Provinces", "p", "Id", "Name", "RegionId", 2, null, null, null)
            };
            var joinQuery = CoreService.CreateJoinQueryForHierarchy(currentQuery, rootTable, lowerLevelTables);
            var expectedResult = currentQuery + "\n\tJOIN [Provinces] AS p ON r.[Id] = p.[RegionId]";
            Assert.AreEqual(expectedResult, joinQuery);
        }

        [TestCategory("CreateJoinQueryForHierarchy")]
        [TestMethod]
        public void TwoLowerLevels()
        {
            var currentQuery = "SELECT r.[Id] FROM [Regions] AS r";
            var rootTable = new AccessAttributeTable(1, "Regions", "r", "Id", "Name", null, 1, null, null, null);
            var lowerLevelTables = new List<AccessAttributeTable>
            {
                new AccessAttributeTable(2, "Provinces", "p", "Id", "Name", "RegionId", 2, null, null, null),
                new AccessAttributeTable(3, "Stores", "s", "Id", "Name", "ProvinceId", 3, null, null, null)
            };
            var joinQuery = CoreService.CreateJoinQueryForHierarchy(currentQuery, rootTable, lowerLevelTables);
            var expectedResult = currentQuery
                + "\n\tJOIN [Provinces] AS p ON r.[Id] = p.[RegionId]"
                + "\n\tJOIN [Stores] AS s ON p.[Id] = s.[ProvinceId]";
            Assert.AreEqual(expectedResult, joinQuery);
        }
    }
}