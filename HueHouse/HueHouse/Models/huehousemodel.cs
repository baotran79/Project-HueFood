namespace HueHouse.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class huehousemodel : DbContext
    {
        public huehousemodel()
            : base("name=huehousemodel")
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<AdminManagement> AdminManagement { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<HighlightedProducts> HighlightedProducts { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<OrderHistory> OrderHistory { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<ProductDetails> ProductDetails { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<UnifiedContactFeedback> UnifiedContactFeedback { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .HasMany(e => e.AdminManagement)
                .WithRequired(e => e.Admin)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Categories>()
            .HasMany(c => c.Products) // Một danh mục có nhiều sản phẩm
            .WithRequired(p => p.Category) // Một sản phẩm yêu cầu thuộc về một danh mục
            .HasForeignKey(p => p.CategoryID) // Khóa ngoại ở bảng Products
            .WillCascadeOnDelete(false); // Không xóa danh mục khi xóa sản phẩm


            modelBuilder.Entity<Orders>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Orders)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Orders>()
                .HasMany(e => e.OrderHistory)
                .WithRequired(e => e.Orders)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Orders>()
                .HasMany(e => e.Payments)
                .WithRequired(e => e.Orders)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.Cart)
                .WithRequired(e => e.Products)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.HighlightedProducts)
                .WithRequired(e => e.Products)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Products)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.ProductDetails)
                .WithRequired(e => e.Products)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Cart)
                .WithRequired(e => e.Users)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.OrderHistory)
                .WithRequired(e => e.Users)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Users)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.UnifiedContactFeedback)
                .WithRequired(e => e.Users)
                .WillCascadeOnDelete(false);
        }
    }
}
