using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Base
{
    public class BaseModel<TKey>
    {
        public BaseModel()
        {
            Id = default!;
        }

        public BaseModel(TKey id)
        {
            Id = id;
        }

        public TKey Id { get; internal set; }
    }
}
