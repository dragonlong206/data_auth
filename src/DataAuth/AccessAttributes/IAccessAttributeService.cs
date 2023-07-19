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
    }
}
