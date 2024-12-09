namespace HueHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            Cart = new HashSet<Cart>();
            HighlightedProducts = new HashSet<HighlightedProducts>();
            OrderDetails = new HashSet<OrderDetails>();
            ProductDetails = new HashSet<ProductDetails>();
        }

        [Key]
        public int ProductID { get; set; }

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        public int Price { get; set; }

        public int CategoryID { get; set; }

        // Thuộc tính liên kết với Category
        public virtual Categories Category { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Cart { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HighlightedProducts> HighlightedProducts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductDetails> ProductDetails { get; set; }
    }
}
