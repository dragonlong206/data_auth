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
        public async Task<DataPermissionResult<int>> GetDataPermission(
            string subjectId,
            string accessAttributeCode,
            GrantType grantType = GrantType.ForUser,
            string? localLookupValue = null
        )
        {
            return await _coreService.GetDataPermissions<int>(
                subjectId,
                accessAttributeCode,
                grantType,
                localLookupValue
            );
        }

        [Route("OrderTypes")]
        [HttpGet]
        public async Task<DataPermissionResult<Guid>> GetGrantedOrderType(
            string subjectId,
            GrantType grantType = GrantType.ForUser,
            string? localLookupValue = null,
            CancellationToken cancellationToken = default
        )
        {
            return await _coreService.GetDataPermissions<Guid>(
                subjectId,
                "ORDER_TYPE",
                grantType,
                localLookupValue,
                cancellationToken
            );
        }
    }
}
