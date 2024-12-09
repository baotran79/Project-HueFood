namespace HueHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetails
    {
        [Key]
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }

        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? TotalAmount { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual Orders Orders { get; set; }

        public virtual Products Products { get; set; }
    }
}
