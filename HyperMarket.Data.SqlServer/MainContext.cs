using System;
using HyperMarket.Data.SqlServer.Maps;
using Microsoft.EntityFrameworkCore;
using HyperMarket.DomainObjects;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HyperMarket.Data.SqlServer
{
    public class MainContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<DoubleProperty> DoubleProperties { get; set; }
        public DbSet<DoublePropertyValue> DoublePropertyValues { get; set; }
        public DbSet<DoubleValue> DoubleValues { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<PhoneConfirmation> PhoneConfirmations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductDoubleProperty> ProductDoubleProperties { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<ProductStringProperty> ProductStringProperties { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StringProperty> StringProperties { get; set; }
        public DbSet<StringPropertyValue> StringPropertyValues { get; set; }
        public DbSet<StringValue> StringValues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<UserStorePermission> UserStorePermissions { get; set; }
        public DbSet<UserStore> UserStores { get; set; }
        public MainContext() { }
        public MainContext(DbContextOptions<MainContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AddressMap());
            modelBuilder.ApplyConfiguration(new CartMap());
            modelBuilder.ApplyConfiguration(new CartItemMap());
            modelBuilder.ApplyConfiguration(new DoublePropertyMap());
            modelBuilder.ApplyConfiguration(new DoublePropertyValueMap());
            modelBuilder.ApplyConfiguration(new DoubleValueMap());
            modelBuilder.ApplyConfiguration(new ManufacturerMap());
            modelBuilder.ApplyConfiguration(new OrderMap());
            modelBuilder.ApplyConfiguration(new OrderItemMap());
            modelBuilder.ApplyConfiguration(new PhoneConfirmationMap());
            modelBuilder.ApplyConfiguration(new ProductMap());
            modelBuilder.ApplyConfiguration(new ProductCategoryMap());
            modelBuilder.ApplyConfiguration(new ProductDoublePropertyMap());
            modelBuilder.ApplyConfiguration(new ProductStockMap());
            modelBuilder.ApplyConfiguration(new ProductStringPropertyMap());
            modelBuilder.ApplyConfiguration(new StoreMap());
            modelBuilder.ApplyConfiguration(new StringPropertyMap());
            modelBuilder.ApplyConfiguration(new StringPropertyValueMap());
            modelBuilder.ApplyConfiguration(new StringValueMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new UserAddressMap());
            modelBuilder.ApplyConfiguration(new UserPermissionMap());
            modelBuilder.ApplyConfiguration(new UserStorePermissionMap());
            modelBuilder.ApplyConfiguration(new UserStoreMap());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
            }
        }
    }
}
