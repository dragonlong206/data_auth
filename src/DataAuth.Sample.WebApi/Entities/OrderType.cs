using DataAuth.Base;

namespace DataAuth.Sample.WebApi.Entities
{
    public class OrderType : BaseEntity<Guid>
    {
        public OrderType(string name, string code, Guid orderTypeGroupId) : base(Guid.NewGuid())
        {
            Name = name;
            Code = code;
            OrderTypeGroupId = orderTypeGroupId;
        }

        public OrderType(Guid id, string name, string code, Guid orderTypeGroupId) : base(id)
        {
            Name = name;
            Code = code;
            OrderTypeGroupId = orderTypeGroupId;
        }

        public string Name { get; set; }
        public string Code { get; set; }

        public Guid OrderTypeGroupId { get; set; }

        public OrderTypeGroup? Group { get; set; }
    }
}
