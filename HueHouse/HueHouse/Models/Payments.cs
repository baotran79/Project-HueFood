namespace HueHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Payments
    {
        [Key]
        public int PaymentID { get; set; }

        public int OrderID { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? PaymentProcessedAt { get; set; }

        public virtual Orders Orders { get; set; }
    }
}
