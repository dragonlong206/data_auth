using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.DataAccessService.Interface
{
    public interface IDataAuthService
    {
        Task<IEnumerable<DataPermission>> GetDataPermissions(string subjectId, GrantType grantType);
    }
}
