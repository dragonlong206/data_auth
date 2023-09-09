using DataAuth.Cache;
using DataAuth.Core;
using DataAuth.Domains.AccessAttributes;
using DataAuth.Domains.AccessAttributeTables;
using DataAuth.Domains.DataPermissions;
using DataAuth.Entities;
using DataAuth.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAuth.Test.UnitTest
{
    [TestClass]
    public class CoreServiceTest
    {
        DataAuthDbContext _dbContext;
        ICoreService _coreService;
        ICacheProvider _cacheProvider;

        public CoreServiceTest()
        {
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: "DataAuthTest")
                .Options;
            _dbContext = new DataAuthDbContext(options);
            _cacheProvider = new MemoryCacheProvider(
                new MemoryCache(new MemoryCacheOptions { SizeLimit = 1024 })
            );
            _coreService = new CoreService(_dbContext, _cacheProvider);
        }

        [TestCategory("GetDataPermissions")]
        [TestMethod]
        public async Task UsingCacheProvider()
        {
            var subjectId = Guid.NewGuid().ToString();
            var accessAttributeCode = "TEST";
            var grantType = GrantType.ForUser;
            var cacheKey = CoreService.GetCacheKey(subjectId, accessAttributeCode, grantType);

            var mock = new Mock<ICacheProvider>();

            var coreService = new CoreService(_dbContext, mock.Object);
            await coreService.GetDataPermissions<int>(subjectId, accessAttributeCode, grantType);

            mock.Verify(c => c.Get<DataPermissionResult<int>>(cacheKey), Times.Once);
        }

        [TestCategory("GetDataPermissions")]
        [TestMethod]
        public async Task GetDataAndSetToCacheWhenCacheEmpty()
        {
            var subjectId = Guid.NewGuid().ToString();
            var accessAttributeCode = "TEST";
            var grantType = GrantType.ForUser;
            var cacheKey = CoreService.GetCacheKey(subjectId, accessAttributeCode, grantType);

            var cacheProviderMock = new Mock<ICacheProvider>();
            cacheProviderMock
                .Setup(x => x.Get<DataPermissionResult<int>>(cacheKey))
                .Returns((DataPermissionResult<int>)null);

            var coreService = new CoreService(_dbContext, cacheProviderMock.Object);
            var permissions = await coreService.GetDataPermissions<int>(
                subjectId,
                accessAttributeCode,
                grantType
            );

            cacheProviderMock.Verify(c => c.Get<DataPermissionResult<int>>(cacheKey), Times.Once);
            cacheProviderMock.Verify(c => c.Set(cacheKey, permissions, 24), Times.Once);
        }

        [TestCategory("GetDataPermissions")]
        [TestMethod]
        public async Task NotSetDataToCacheWhenCacheHaveValue()
        {
            var subjectId = Guid.NewGuid().ToString();
            var accessAttributeCode = "TEST";
            var grantType = GrantType.ForUser;
            var cacheKey = CoreService.GetCacheKey(subjectId, accessAttributeCode, grantType);

            var cacheProviderMock = new Mock<ICacheProvider>();
            cacheProviderMock
                .Setup(x => x.Get<DataPermissionResult<int>>(cacheKey))
                .Returns(new DataPermissionResult<int>());

            var coreService = new CoreService(_dbContext, cacheProviderMock.Object);
            var permissions = await coreService.GetDataPermissions<int>(
                subjectId,
                accessAttributeCode,
                grantType
            );

            cacheProviderMock.Verify(c => c.Get<DataPermissionResult<int>>(cacheKey), Times.Once);
            cacheProviderMock.Verify(c => c.Set(cacheKey, permissions, 24), Times.Never);
        }

        private async Task<AccessAttributeTableModel> InitAccessAttribute(
            string accessAttributeCode
        )
        {
            var accessAttributeService = new AccessAttributeService(_dbContext);
            var accessAttribute = new AccessAttributeModel(accessAttributeCode);
            await accessAttributeService.AddAccessAttribute(accessAttribute);

            var accessAttributeTableService = new AccessAttributeTableService(_dbContext);
            var accessAttributeTable = new AccessAttributeTableModel(
                accessAttribute.Id,
                // Due to using EF, this table always exists.
                "__EFMigrationsHistory",
                "h",
                "MigrationId",
                "MigrationId",
                false,
                null,
                1,
                false,
                null,
                null,
                null
            );
            await accessAttributeTableService.AddAccessAttributeTable(accessAttributeTable);
            return accessAttributeTable;
        }

        [TestCategory("GetDataPermissions")]
        [TestMethod]
        public async Task GetDataPermissionByFunctionCode()
        {
            // Insert 2 different permissions for 2 differrent function codes, get data permission by one function code
            // Assert that result match with the passing function code
            var subjectId = Guid.NewGuid().ToString();
            var accessAttributeCode = "TEST";

            var accessAttributeTable = await InitAccessAttribute(accessAttributeCode);

            var dataPermissionService = new DataPermissionService(_dbContext, _cacheProvider);
            var dataPermission1 = new DataPermissionModel(subjectId, FunctionCode.Update)
            {
                AccessAttributeTableId = accessAttributeTable.Id,
                GrantType = GrantType.ForUser,
                AccessLevel = AccessLevel.Specific,
                GrantedDataValue = "1"
            };
            await dataPermissionService.AddDataPermission(dataPermission1);

            var dataPermission2 = new DataPermissionModel(subjectId, FunctionCode.Read)
            {
                AccessAttributeTableId = accessAttributeTable.Id,
                GrantType = GrantType.ForUser,
                AccessLevel = AccessLevel.Global
            };
            await dataPermissionService.AddDataPermission(dataPermission2);

            var permissionResult = await _coreService.GetDataPermissions<string>(
                subjectId,
                accessAttributeCode,
                GrantType.ForUser,
                null,
                FunctionCode.Update,
                default
            );

            Assert.IsNotNull(permissionResult);
            Assert.AreEqual(1, permissionResult.PermissionDetails.Count());
            Assert.AreEqual(dataPermission1.Id, permissionResult.PermissionDetails.ElementAt(0).Id);
        }

        [TestCategory("GenerateQuery")]
        [TestMethod]
        public async Task GlobalLevel()
        {
            var permission = new DataPermission(
                Enums.GrantType.ForUser,
                "1",
                1,
                Enums.AccessLevel.Global
            );
            permission.AccessAttributeTable = new AccessAttributeTable(
                1,
                "Stores",
                "s",
                "Id",
                "Name"
            );
            var query = await _coreService.GenerateQueryString(permission);
            Assert.AreEqual("SELECT s.[Id] FROM [Stores] AS s", query.QueryString);
        }

        [TestCategory("GenerateQuery")]
        [TestMethod]
        public async Task LocalLevel()
        {
            var permission = new DataPermission(
                Enums.GrantType.ForUser,
                "1",
                1,
                Enums.AccessLevel.Local
            );
            permission.AccessAttributeTable = new AccessAttributeTable(
                1,
                "Stores",
                "s",
                "Id",
                "Name",
                false,
                null,
                1,
                true,
                "Employees",
                "Id",
                "LastWorkingStoreId"
            );
            var lookupValue = "999";
            var query = await _coreService.GenerateQueryString(permission, lookupValue);
            Assert.AreEqual(
                "SELECT s.[Id] FROM [Stores] AS s\nWHERE s.[Id] IN (SELECT [LastWorkingStoreId] FROM [Employees] WHERE [Id] = @LookupValue)",
                query.QueryString
            );
            Assert.IsNotNull(query.QueryParams);
            Assert.AreEqual(1, query.QueryParams.Count);
            Assert.AreEqual("LookupValue", query.QueryParams[0].ParameterName);
            Assert.AreEqual(lookupValue, query.QueryParams[0].Value);
        }

        [TestCategory("GenerateQuery")]
        [TestMethod]
        public async Task SpecificLevel()
        {
            var permission = new DataPermission(
                Enums.GrantType.ForUser,
                "1",
                1,
                Enums.AccessLevel.Specific,
                "999"
            );
            permission.AccessAttributeTable = new AccessAttributeTable(
                1,
                "Stores",
                "s",
                "Id",
                "Name",
                false,
                null,
                1,
                true,
                "Employees",
                "Id",
                "LastWorkingStoreId"
            );
            var query = await _coreService.GenerateQueryString(permission);
            Assert.AreEqual(
                "SELECT s.[Id] FROM [Stores] AS s\nWHERE s.[Id] = @LookupValue",
                query.QueryString
            );
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
            var permission = new DataPermission(
                Enums.GrantType.ForUser,
                "1",
                1,
                Enums.AccessLevel.Deep,
                lookupValue
            )
            {
                AccessAttributeTable = new AccessAttributeTable(
                    1,
                    "Departments",
                    "d",
                    "Id",
                    "Name",
                    true,
                    "ParentDepartmentId",
                    null
                )
            };
            var query = await _coreService.GenerateQueryString(permission);
            var expectedQuery =
                ";WITH cte AS"
                + $"\n(SELECT [Id], [ParentDepartmentId] FROM [Departments] WHERE [Id] = @LookupValue"
                + $"\nUNION ALL"
                + $"\nSELECT t2.[Id], t2.[ParentDepartmentId] FROM cte t1"
                + $"\nJOIN [Departments] t2 ON t1.[Id] = t2.[ParentDepartmentId])"
                + $"\nSELECT [Id] FROM cte"
                + $"\nOPTION (MAXRECURSION 20)";
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
            var attribute = new AccessAttributeModel("STORE");
            var accessAttributeService = new AccessAttributeService(_dbContext);
            await accessAttributeService.AddAccessAttribute(attribute);

            var accessAttributeTableService = new AccessAttributeTableService(_dbContext);
            var attributeTable1 = new AccessAttributeTableModel(
                attribute.Id,
                "Regions",
                "r",
                "Id",
                "Name",
                false,
                null,
                1,
                false,
                null,
                null,
                null
            );
            await accessAttributeTableService.AddAccessAttributeTable(attributeTable1);
            var attributeTable2 = new AccessAttributeTableModel(
                attribute.Id,
                "Provinces",
                "p",
                "Id",
                "Name",
                false,
                "RegionId",
                2,
                false,
                null,
                null,
                null
            );
            await accessAttributeTableService.AddAccessAttributeTable(attributeTable2);
            var attributeTable3 = new AccessAttributeTableModel(
                attribute.Id,
                "Stores",
                "s",
                "Id",
                "Name",
                false,
                "ProvinceId",
                3,
                true,
                null,
                null,
                null
            );
            await accessAttributeTableService.AddAccessAttributeTable(attributeTable3);
            var rootAttributeTable =
                await accessAttributeTableService.GetAccessAttributeTableEntityById(
                    attributeTable1.Id
                );

            // Exec
            var lookupValue = "999";
            var permission = new DataPermission(
                Enums.GrantType.ForUser,
                "1",
                attributeTable3.Id,
                Enums.AccessLevel.Deep,
                lookupValue
            )
            {
                AccessAttributeTable = rootAttributeTable
            };
            var (QueryString, QueryParams) = await _coreService.GenerateQueryString(permission);

            var expectedResult =
                "SELECT s.[Id] FROM [Regions] AS r"
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
            var joinQuery = CoreService.CreateJoinQueryForHierarchy(
                currentQuery,
                rootTable,
                new List<AccessAttributeTable>()
            );
            Assert.AreEqual(currentQuery, joinQuery);
        }

        [TestCategory("CreateJoinQueryForHierarchy")]
        [TestMethod]
        public void OneLowerLevel()
        {
            var currentQuery = "SELECT r.[Id] FROM [Regions] AS r";
            var rootTable = new AccessAttributeTable(
                1,
                "Regions",
                "r",
                "Id",
                "Name",
                false,
                null,
                1,
                false,
                null,
                null,
                null
            );
            var lowerLevelTables = new List<AccessAttributeTable>
            {
                new AccessAttributeTable(
                    2,
                    "Provinces",
                    "p",
                    "Id",
                    "Name",
                    false,
                    "RegionId",
                    2,
                    false,
                    null,
                    null,
                    null
                )
            };
            var joinQuery = CoreService.CreateJoinQueryForHierarchy(
                currentQuery,
                rootTable,
                lowerLevelTables
            );
            var expectedResult =
                currentQuery + "\n\tJOIN [Provinces] AS p ON r.[Id] = p.[RegionId]";
            Assert.AreEqual(expectedResult, joinQuery);
        }

        [TestCategory("CreateJoinQueryForHierarchy")]
        [TestMethod]
        public void TwoLowerLevels()
        {
            var currentQuery = "SELECT r.[Id] FROM [Regions] AS r";
            var rootTable = new AccessAttributeTable(
                1,
                "Regions",
                "r",
                "Id",
                "Name",
                false,
                null,
                1,
                false,
                null,
                null,
                null
            );
            var lowerLevelTables = new List<AccessAttributeTable>
            {
                new AccessAttributeTable(
                    2,
                    "Provinces",
                    "p",
                    "Id",
                    "Name",
                    false,
                    "RegionId",
                    2,
                    false,
                    null,
                    null,
                    null
                ),
                new AccessAttributeTable(
                    3,
                    "Stores",
                    "s",
                    "Id",
                    "Name",
                    false,
                    "ProvinceId",
                    3,
                    true,
                    null,
                    null,
                    null
                )
            };
            var joinQuery = CoreService.CreateJoinQueryForHierarchy(
                currentQuery,
                rootTable,
                lowerLevelTables
            );
            var expectedResult =
                currentQuery
                + "\n\tJOIN [Provinces] AS p ON r.[Id] = p.[RegionId]"
                + "\n\tJOIN [Stores] AS s ON p.[Id] = s.[ProvinceId]";
            Assert.AreEqual(expectedResult, joinQuery);
        }
    }
}
