using DataAuth.AccessAttributes;
using DataAuth.AccessAttributeTables;
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
        DataAuthDbContext _dbContext;
        ICoreService _coreService;

        public CoreServiceTest()
        {
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: "DataAuthTest")
                .Options;
            _dbContext = new DataAuthDbContext(options);
            _coreService = new CoreService(_dbContext);
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
            var lookupValue = "999";
            var query = await _coreService.GenerateQueryString(permission, lookupValue);
            Assert.AreEqual("SELECT s.[Id] FROM [Stores] AS s\nWHERE s.[Id] IN (SELECT [LastWorkingStoreId] FROM [Employees] WHERE [Id] = @LookupValue)", query.QueryString);
            Assert.IsNotNull(query.QueryParams);
            Assert.AreEqual(1, query.QueryParams.Count);
            Assert.AreEqual("LookupValue", query.QueryParams[0].ParameterName);
            Assert.AreEqual(lookupValue, query.QueryParams[0].Value);
        }

        [TestCategory("GenerateQuery")]
        [TestMethod]
        public async Task SpecificLevel()
        {
            var permission = new DataPermission(Enums.GrantType.ForUser, "1", 1, Enums.AccessLevel.Specific, "999");
            permission.AccessAttributeTable = new AccessAttributeTable(1, "Stores", "s", "Id", "Name", null, 1, "Employees", "Id", "LastWorkingStoreId");
            var query = await _coreService.GenerateQueryString(permission);
            Assert.AreEqual("SELECT s.[Id] FROM [Stores] AS s\nWHERE s.[Id] = @LookupValue", query.QueryString);
            Assert.IsNotNull(query.QueryParams);
            Assert.AreEqual(1, query.QueryParams.Count);
            Assert.AreEqual("LookupValue", query.QueryParams[0].ParameterName);
            Assert.AreEqual("999", query.QueryParams[0].Value);
        }

        [TestCategory("GenerateQuery")]
        [TestMethod]
        public async Task DeepLevel_SelfReference()
        {
            var lookupValue = "999";
            var permission = new DataPermission(Enums.GrantType.ForUser, "1", 1, Enums.AccessLevel.Deep, lookupValue)
            {
                AccessAttributeTable = new AccessAttributeTable(1, "Departments", "d", "Id", "Name", "ParentDepartmentId", null)
                {
                    IsSelfReference = true
                }
            };
            var query = await _coreService.GenerateQueryString(permission);
            var expectedQuery = ";WITH cte AS" +
                $"(SELECT [Id], [ParentDepartmentId] FROM [Departments] WHERE [Id] = @LookupValue" +
                $"UNION ALL" +
                $"SELECT t2.[Id], t2.[ParentDepartmentId] FROM cte t1" +
                $"JOIN [Departments] t2 ON t1.[Id] = t2.[ParentDepartmentId])" +
                $"SELECT [Id] FROM cte" +
                $"OPTION (MAXRECURSION 20)"; ;
            Assert.AreEqual(expectedQuery, query.QueryString);
            Assert.IsNotNull(query.QueryParams);
            Assert.AreEqual(1, query.QueryParams.Count);
            Assert.AreEqual("LookupValue", query.QueryParams[0].ParameterName);
            Assert.AreEqual(lookupValue, query.QueryParams[0].Value);
        }

        [TestCategory("GenerateQuery")]
        [TestMethod]
        public async Task DeepLevel_LevelSeparatedHierarchy()
        {
            // Initialize
            var attribute = new AccessAttributeModel
            {
                Code = "STORE"
            };
            var accessAttributeService = new AccessAttributeService(_dbContext);
            await accessAttributeService.AddAccessAttribute(attribute);

            var accessAttributeTableService = new AccessAttributeTableService(_dbContext);
            var attributeTable1 = new AccessAttributeTableModel(attribute.Id, "Regions", "r", "Id", "Name", null, 1, null, null, null);
            await accessAttributeTableService.AddAccessAttributeTable(attributeTable1);
            var attributeTable2 = new AccessAttributeTableModel(attribute.Id, "Provinces", "p", "Id", "Name", "RegionId", 2, null, null, null);
            await accessAttributeTableService.AddAccessAttributeTable(attributeTable2);
            var attributeTable3 = new AccessAttributeTableModel(attribute.Id, "Stores", "s", "Id", "Name", "ProvinceId", 3, null, null, null);
            await accessAttributeTableService.AddAccessAttributeTable(attributeTable3);
            var rootAttributeTable = await accessAttributeTableService.GetAccessAttributeTable(attributeTable1.Id);

            // Exec
            var lookupValue = "999";
            var permission = new DataPermission(Enums.GrantType.ForUser, "1", attributeTable3.Id, Enums.AccessLevel.Deep, lookupValue)
            {
                AccessAttributeTable = rootAttributeTable
            };
            var (QueryString, QueryParams) = await _coreService.GenerateQueryString(permission);

            var expectedResult = "SELECT r.[Id] FROM [Regions] AS r"
                + "\n\tJOIN [Provinces] AS p ON r.[Id] = p.[RegionId]"
                + "\n\tJOIN [Stores] AS s ON p.[Id] = s.[ProvinceId]"
                + "\nWHERE r.[Id] = @LookupValue";
            Assert.AreEqual(expectedResult, QueryString);
            Assert.IsNotNull(QueryParams);
            Assert.AreEqual(1, QueryParams.Count);
            Assert.AreEqual("LookupValue", QueryParams[0].ParameterName);
            Assert.AreEqual(lookupValue, QueryParams[0].Value);
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