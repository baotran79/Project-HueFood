namespace HueHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class ProductDetailsViewModel
    {
        public Products Products { get; set; }
        public ProductDetails ProductDetails { get; set; }
    }
}
