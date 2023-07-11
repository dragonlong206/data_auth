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
            SeedSampleData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void SeedSampleData(ModelBuilder modelBuilder)
        {
            var region1 = new Region("NT", "Northern") { Id = 1 };
            var region2 = new Region("CT", "Central") { Id = 2 };
            var region3 = new Region("ST", "Southern") { Id = 3 };
            modelBuilder.Entity<Region>()
                .HasData(region1, region2, region3);


            var province1 = new Province("Ho Chi Minh", "HCM", region3.Id) { Id = 1 };
            var province2 = new Province("Tay Ninh", "TNI", region3.Id) { Id = 2 };
            var province3 = new Province("Binh Duong", "BDU", region3.Id) { Id = 3 };
            var province4 = new Province("Đồng Nai", "DNI", region3.Id) { Id = 4 };
            modelBuilder.Entity<Province>()
                .HasData(province1, province2, province3, province4);

            var store1 = new Store("Tran Hung Dao store", "HCM_001", province1.Id) { Id = 1 };
            var store2 = new Store("Nguyen Van Cu store", "HCM_002", province1.Id) { Id = 2 };
            var store3 = new Store("Nguyen Oanh store", "HCM_003", province1.Id) { Id = 3 };
            var store4 = new Store("Phan Dang Luu store", "HCM_004", province1.Id) { Id = 4 };
            var store5 = new Store("Vo Van Viet store", "HCM_005", province1.Id) { Id = 5 };
            var store6 = new Store("Bien Hoa 1 store", "DNI_001", province4.Id) { Id = 6 };
            var store7 = new Store("Di An store", "BDU_001", province3.Id) { Id = 7 };
            modelBuilder.Entity<Store>()
                .HasData(store1, store2, store3, store4, store5, store6, store7);

            var department1 = new Department("Sales", "KDO") { Id = 1 };
            var department2 = new Department("Store selling", "KDO_CHA", department1.Id) { Id = 2 };
            var department3 = new Department("Sale back office", "KDO_NVU", department2.Id) { Id = 3 };
            var department4 = new Department("Supplier management", "KDO_NVU_VTU", department3.Id) { Id = 4 };
            var department5 = new Department("Document management", "KDO_NVU_CTU", department3.Id) { Id = 5 };
            var department6 = new Department("Seller", "KDO_CHA_NVB", department2.Id) { Id = 6 };
            modelBuilder.Entity<Department>()
                .HasData(department1, department2, department3, department4, department5, department6);

            var orderTypeGroup1 = new OrderTypeGroup(new Guid("6edc2d8c-ada9-48ea-b347-72a2832607ed"), "Offline retail", "BL");
            var orderTypeGroup2 = new OrderTypeGroup(new Guid("f829ca08-1958-42d8-9aa1-812599b5a9de"), "Offline wholesale", "DL");
            modelBuilder.Entity<OrderTypeGroup>()
                .HasData(orderTypeGroup1, orderTypeGroup2);

            var orderType1 = new OrderType(new Guid("81c23610-904a-48f3-b0c0-4062d4c5dc15"), "Store retailing", "DHCH", orderTypeGroup1.Id);
            var orderType2 = new OrderType(new Guid("bfd95cac-1c38-4b31-8a49-59521ec84338"), "Agency wholesale", "DHDL", orderTypeGroup1.Id);
            modelBuilder.Entity<OrderType>()
                .HasData(orderType1, orderType2);
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Department> Departments{ get; set; }

        public DbSet<OrderTypeGroup> OrderTypeGroups { get; set; }

        public DbSet<OrderType> OrderTypes { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}
