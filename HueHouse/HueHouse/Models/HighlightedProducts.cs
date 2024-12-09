    namespace HueHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HighlightedProducts
    {
        [Key]
        public int HighlightID { get; set; }

        public int ProductID { get; set; }

        [Required]
        [StringLength(50)]
        public string HighlightType { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual Products Products { get; set; }
    }
}
