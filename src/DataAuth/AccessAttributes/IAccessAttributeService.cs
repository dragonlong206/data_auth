using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.AccessAttributes
{
    public interface IAccessAttributeService
    {
        Task<AccessAttributeModel> AddAccessAttribute(
            AccessAttributeModel model,
            CancellationToken cancellationToken = default
        );

        Task<AccessAttributeModel> UpdateAccessAttribute(
            int id,
            AccessAttributeModel model,
            CancellationToken cancellationToken = default
        );

        Task DeleteAccessAttribute(int id, CancellationToken cancellationToken = default);

        Task<AccessAttributeModel?> GetAccessAttributeById(
            int id,
            CancellationToken cancellationToken = default
        );

        Task<IEnumerable<AccessAttributeModel>> GetAccessAttributes(
            CancellationToken cancellationToken = default
        );
    }
}
