using DataAuth.Sample.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAuth.Sample.WebApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Seeding data
            var region1 = new Region("MB", "Miền Bắc") { Id = 1 };
            var region2 = new Region("MT", "Miền Trung") { Id = 2 };
            var region3 = new Region("MN", "Miền Nam") { Id = 3 };
            modelBuilder.Entity<Region>()
                .HasData(region1, region2, region3);


            var province1 = new Province("Hồ Chí Minh", "HCM", region3.Id) { Id = 1 };
            var province2 = new Province("Tây Ninh", "TNI", region3.Id) { Id = 2 };
            var province3 = new Province("Bình Dương", "BDU", region3.Id) { Id = 3 };
            var province4 = new Province("Đồng Nai", "DNI", region3.Id) { Id = 4 };
            modelBuilder.Entity<Province>()
                .HasData(province1, province2, province3, province4);

            var store1 = new Store("Cửa hàng Trần Hưng Đạo", "HCM_001", province1.Id) { Id = 1 };
            var store2 = new Store("Cửa hàng Nguyễn Văn Cừ", "HCM_002", province1.Id) { Id = 2 };
            var store3 = new Store("Cửa hàng Nguyễn Oanh", "HCM_003", province1.Id) { Id = 3 };
            var store4 = new Store("Cửa hàng Phan Đăng Lưu", "HCM_004", province1.Id) { Id = 4 };
            var store5 = new Store("Cửa hàng Võ Văn Việt", "HCM_005", province1.Id) { Id = 5 };
            var store6 = new Store("Cửa hàng Biên Hòa 1", "DNI_001", province4.Id) { Id = 6 };
            var store7 = new Store("Cửa hàng Dĩ An", "BDU_001", province3.Id) { Id = 7 };
            modelBuilder.Entity<Store>()
                .HasData(store1, store2, store3, store4, store5, store6, store7);

            var department1 = new Department("Phòng Kinh doanh", "KDO") { Id = 1 };
            var department2 = new Department("Bộ phận Cửa hàng", "KDO_CHA", department1.Id) { Id = 2 };
            var department3 = new Department("Bộ phận Nghiệp vụ", "KDO_NVU", department2.Id) { Id = 3 };
            var department4 = new Department("Bộ phận Vật tư", "KDO_NVU_VTU", department3.Id) { Id = 4 };
            var department5 = new Department("Bộ phận Chứng từ hóa đơn", "KDO_NVU_CTU", department3.Id) { Id = 5 };
            var department6 = new Department("Nhân viên bán hàng", "KDO_CHA_NVB", department2.Id) { Id = 6 };
            modelBuilder.Entity<Department>()
                .HasData(department1, department2, department3, department4, department5, department6);
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Department> Departments{ get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}
