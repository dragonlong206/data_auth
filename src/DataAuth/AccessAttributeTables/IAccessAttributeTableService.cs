using DataAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.AccessAttributeTables
{
    public interface IAccessAttributeTableService
    {
        Task AddAccessAttributeTable(
            AccessAttributeTableModel model,
            CancellationToken cancellationToken = default
        );

        Task UpdateAccessAttributeTable(
            int id,
            AccessAttributeTableModel model,
            CancellationToken cancellationToken = default
        );

        Task DeleteAccessAttributeTable(int id, CancellationToken cancellationToken = default);

        Task<AccessAttributeTableModel?> GetAccessAttributeTableById(
            int id,
            CancellationToken cancellationToken = default
        );

        Task<AccessAttributeTable?> GetAccessAttributeTableEntityById(
            int id,
            CancellationToken cancellationToken = default
        );

        Task<IEnumerable<AccessAttributeTableModel>> GetAccessAttributeTables(
            CancellationToken cancellationToken = default
        );
    }
}
