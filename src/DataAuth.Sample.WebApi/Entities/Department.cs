using DataAuth.Base;

namespace DataAuth.Sample.WebApi.Entities
{
    public class Department : BaseEntity<int>
    {
        public Department(string name, string code, int? parentDepartmentId = null)
        {
            Name = name;
            Code = code;
            ParentDepartmentId = parentDepartmentId;
        }

        public string Name { get; set; }

        public string Code { get; set; }

        public int? ParentDepartmentId { get; set; }

        public Department? ParentDepartment { get; set; }
    }
}
