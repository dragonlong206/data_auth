using DataAuth.Base;

namespace DataAuth.Sample.WebApi.Entities
{
    public class Province : BaseEntity<int>
    {
        public Province(string name, string code, int regionId)
        {
            Name = name;
            Code = code;
            RegionId = regionId;
        }

        public string Name { get; set; }
        public string Code { get; set; }

        public int RegionId { get; set; }
        public Region? Region { get; set; }
    }
}
