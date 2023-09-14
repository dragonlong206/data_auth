using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Base
{
    public class BaseEntity<TKey>
    {
        public BaseEntity()
        {
            Id = default(TKey);
        }

        public BaseEntity(TKey id)
        {
            Id = id;
        }

        public TKey Id { get; set; }
    }
}
