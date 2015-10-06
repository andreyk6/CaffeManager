namespace CaffeManagerServer.Context
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;
    using Model;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;

    public class CaffeDbContext : DbContext
    {
        public DbSet<Cashier> Cashiers { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        
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
            var manager1 = new Manager("manager1", "password") { Name = "Manager1" };
            manager1.Cashiers = new List<Cashier>();

            var cashier1 = new Cashier("cashier", "password") { Name = "Cashier1", //Manager = manager1 
            };
            cashier1.Orders = new List<Order>();
            manager1.Cashiers.Add(cashier1);

            var menuItem1 = new MenuItem() { Name = "Soup", Price = 100 };
            var menuItem2 = new MenuItem() { Name = "Soup2", Price = 200 };

            var order1 = new Order();
            order1.OrderItems = new List<OrderItem>();
            cashier1.Orders.Add(order1);

            var order2 = new Order();
            order2.OrderItems = new List<OrderItem>();
            cashier1.Orders.Add(order2);
            
            var orderItem1 = new OrderItem(menuItem1) { Count = 10, //MenuItem = menuItem1, Order = order1 
            };
            order1.OrderItems.Add(orderItem1);
            var orderItem2 = new OrderItem(menuItem2) { Count = 10, //MenuItem = menuItem2, Order = order1 
            };
            order2.OrderItems.Add(orderItem2);

            context.MenuItems.Add(menuItem1);
            context.MenuItems.Add(menuItem2);
            context.OrderItems.Add(orderItem1);
            context.OrderItems.Add(orderItem2);
            context.Orders.Add(order1);
            context.Orders.Add(order2);
            context.Cashiers.Add(cashier1);
            context.Managers.Add(manager1);

            context.SaveChanges();
            //base.Seed(context);
        }
    }
}