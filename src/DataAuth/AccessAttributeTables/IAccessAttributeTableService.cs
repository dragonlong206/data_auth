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
        Task<AccessAttributeTableModel> AddAccessAttributeTable(AccessAttributeTableModel model, CancellationToken cancellationToken = default);
        Task<AccessAttributeTable?> GetAccessAttributeTable(int id, CancellationToken cancellationToken = default);
    }
}
