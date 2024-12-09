namespace HueHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductDetails
    {
        [Key]
        public int ProductDetailID { get; set; }

        public int ProductID { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual Products Products { get; set; }
    }
}
