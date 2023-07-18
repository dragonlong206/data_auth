using DataAuth.Base;

namespace DataAuth.Sample.WebApi.Entities
{
    public class Store : BaseEntity<int>
    {
        public Store(string name, string code, int provinceId)
        {
            Name = name;
            Code = code;
            ProvinceId = provinceId;
        }

        public string Name { get; set; }
        public string Code { get; set; }

        public int ProvinceId { get; set; }
        public Province? Province { get; set; }
    }
}
