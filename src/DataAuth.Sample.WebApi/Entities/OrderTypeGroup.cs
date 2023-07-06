using DataAuth.Base;

namespace DataAuth.Sample.WebApi.Entities
{
    public class OrderTypeGroup : BaseEntity<Guid>
    {
        public OrderTypeGroup(string name, string code) : base(Guid.NewGuid())
        {
            Name = name;
            Code = code;
        }

        public OrderTypeGroup(Guid id, string name, string code) : base(id)
        {
            Name = name;
            Code = code;
        }

        public string Name { get; set; }
        public string Code { get; set; }
    }
}
