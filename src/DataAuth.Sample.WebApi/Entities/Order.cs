using DataAuth.Base;

namespace DataAuth.Sample.WebApi.Entities
{
    public class Order : BaseEntity<int>
    {
        public Order(string orderNumber)
        {
            OrderNumber = orderNumber;
        }

        public string OrderNumber { get; set; }
        public string? CustomerName { get; set; }

        public int? StoreId { get; set; }

        public Store? Store { get; set; }
    }
}
