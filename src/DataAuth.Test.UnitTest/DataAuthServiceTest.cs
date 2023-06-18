using DataAuth.DataAccessService;
using DataAuth.DataAccessService.Implementation;
using DataAuth.DataAccessService.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DataAuth.Test.UnitTest
{
    [TestClass]
    public class DataAuthServiceTest
    {
        IDataAuthService _dataAuthService;

        public DataAuthServiceTest()
        {
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: "DataAuthTest")
                .Options;
            var dbContext = new DataAuthDbContext(options);
            _dataAuthService = new DataAuthService(dbContext);
        }

        [TestMethod]
        public async Task CanAccessTestDatabase()
        {
            var data = await _dataAuthService.GetDataPermissions("1", Domain.Enums.GrantType.ForRole);
            Assert.IsNotNull(data);
        }
    }
}