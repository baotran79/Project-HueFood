namespace HueHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cart")]
    public partial class Cart
    {
        public int CartID { get; set; }

        public int UserID { get; set; }

        public int ProductID { get; set; }

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? TotalAmount { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual Products Products { get; set; }

        public virtual Users Users { get; set; }
    }
}
