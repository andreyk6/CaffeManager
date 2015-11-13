namespace CaffeManagerServer.Context
{
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using CaffeManagerServer.Enitites;
    using CafeManagerLib.SharedModels;

    public class CaffeDbContext : DbContext
    {
        public DbSet<Cashier> Cashiers { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Superuser> Superusers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        private DbSet<CashierStatsModel> zComplexType_CashierStatsModel { get; set; }
        private DbSet<CashierDayStatsModel> zComplexType_CashierDayStatsModel { get; set; }
        private DbSet<CashierMonthStatsModel> zComplexType_CashierMonthStatsModel { get; set; }
        private DbSet<CashierYearStatsModel> zComplexType_CashierYearStatsModel { get; set; }

        public User GetUserByName(string userName)
        {
            User user;
            user = Superusers.ToList().FirstOrDefault(u => u.Login == userName);
            if (user == null)
                user = Managers.ToList().FirstOrDefault(u => u.Login == userName);
            if (user == null)
                user = Cashiers.ToList().FirstOrDefault(u => u.Login == userName);

            return user;
        }
        public CaffeDbContext()
            : base("name=CaffeDatabase")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        static CaffeDbContext()
        {
           Database.SetInitializer(new CaffeModelInitializer());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Superuser>().HasMany(s => s.Managers).WithRequired(m => m.Superuser);  
            modelBuilder.Entity<Manager>().HasMany(m => m.Cashiers).WithRequired(c => c.Manager);
            modelBuilder.Entity<Cashier>().HasMany(c => c.Orders).WithRequired(o => o.Cashier);
            modelBuilder.Entity<Order>().HasMany(o => o.OrderItems).WithRequired(oi => oi.Order);
            modelBuilder.Entity<OrderItem>().HasRequired(oi => oi.MenuItem);

            base.OnModelCreating(modelBuilder);
        }
    }

    public class CaffeModelInitializer : DropCreateDatabaseAlways<CaffeDbContext>
    {
        protected override void Seed(CaffeDbContext context)
        {
            Superuser superuser1 = new Superuser("superuser1", "password") { Name = "SuperUser 1" };
            superuser1.Managers = new List<Manager>();

            var manager1 = new Manager("manager1", "password") { Name = "Manager 1" };
            manager1.Cashiers = new List<Cashier>();
            superuser1.Managers.Add(manager1);
            manager1.Superuser = superuser1;

            var cashier1 = new Cashier("cashier1", "password") { Name = "Cashier 1" };
            cashier1.Orders = new List<Order>();
            manager1.Cashiers.Add(cashier1);

            var cashier2 = new Cashier("cashier2", "password") { Name = "Cashier 2" };
            cashier2.Orders = new List<Order>();
            manager1.Cashiers.Add(cashier2);

            var menuItem1 = new MenuItem() { Name = "Soup", Price = 100 };
            var menuItem2 = new MenuItem() { Name = "Soup2", Price = 200 };
            var menuItem3 = new MenuItem() { Name = "Soup3", Price = 300 };
            var menuItem4 = new MenuItem() { Name = "Soup4", Price = 500 };
            var menuItem5 = new MenuItem() { Name = "Soup5", Price = 50 };
            
            var order1 = new Order();
            order1.OrderItems = new List<OrderItem>();
            order1.DateTime = new System.DateTime(2012, 12, 21);
            cashier1.Orders.Add(order1);

            var order2 = new Order();
            order2.OrderItems = new List<OrderItem>();
            order2.DateTime = new System.DateTime(2012, 12, 21);
            cashier1.Orders.Add(order2);

            var order3 = new Order();
            order3.OrderItems = new List<OrderItem>();
            order3.DateTime = new System.DateTime(2012, 12, 25);
            cashier1.Orders.Add(order3);

            var order4 = new Order();
            order4.OrderItems = new List<OrderItem>();
            order4.DateTime = new System.DateTime(2012, 12, 21);
            cashier2.Orders.Add(order4);

            var order5 = new Order();
            order5.OrderItems = new List<OrderItem>();
            order5.DateTime = new System.DateTime(2012, 12, 24);
            cashier2.Orders.Add(order5);


            var orderItem1 = new OrderItem(menuItem1) { Count = 10 };
            order1.OrderItems.Add(orderItem1);
            order1.CloseOrder();
            var orderItem2 = new OrderItem(menuItem2) { Count = 10 };
            order2.OrderItems.Add(orderItem2);
            order2.CloseOrder();
            var orderItem3 = new OrderItem(menuItem3) { Count = 5 };
            order3.OrderItems.Add(orderItem3);
            order3.CloseOrder();
            var orderItem4 = new OrderItem(menuItem4) { Count = 20 };
            order4.OrderItems.Add(orderItem4);
            order4.CloseOrder();
            var orderItem5 = new OrderItem(menuItem5) { Count = 10 };
            order5.OrderItems.Add(orderItem5);
            order5.CloseOrder();
            
            context.MenuItems.Add(menuItem1);
            context.MenuItems.Add(menuItem2);
            context.MenuItems.Add(menuItem3);
            context.MenuItems.Add(menuItem4);
            context.MenuItems.Add(menuItem5);

            context.Orders.Add(order1);
            context.Orders.Add(order2);
            context.Orders.Add(order3);
            context.Orders.Add(order4);
            context.Orders.Add(order5);

            context.OrderItems.Add(orderItem1);
            context.OrderItems.Add(orderItem2);
            context.OrderItems.Add(orderItem3);
            context.OrderItems.Add(orderItem4);
            context.OrderItems.Add(orderItem5);

            context.Cashiers.Add(cashier1);
            context.Cashiers.Add(cashier2);

            context.Managers.Add(manager1);
            context.Superusers.Add(superuser1);

            context.SaveChanges();
            //base.Seed(context);
        }
    }
}