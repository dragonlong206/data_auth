using DataAuth.Core;
using DataAuth.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataAuth.Sample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataAuthController : ControllerBase
    {
        ICoreService _coreService;
        public DataAuthController(ICoreService coreService)
        {
            _coreService = coreService;
        }

        [HttpGet]
        public async Task<DataPermissionResult<int>> GetStoreDataPermission(string subjectId, GrantType grantType, string accessAttributeCode, string? localLookupValue = null)
        {
            return await _coreService.GetDataPermissions<int>(subjectId, grantType, accessAttributeCode, localLookupValue);
        }
    }
}
