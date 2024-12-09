namespace HueHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderHistory")]
    public partial class OrderHistory
    {
        public int OrderHistoryID { get; set; }

        public int UserID { get; set; }

        public int OrderID { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual Orders Orders { get; set; }

        public virtual Users Users { get; set; }
    }
}
