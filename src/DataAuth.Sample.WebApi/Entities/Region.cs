using DataAuth.Base;

namespace DataAuth.Sample.WebApi.Entities
{
    public class Region : BaseEntity<int>
    {
        public Region(string name, string code)
        {
            Name = name;
            Code = code;

        }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
