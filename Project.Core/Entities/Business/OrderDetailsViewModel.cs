﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Entities.Business
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        [Required]
        public double SellingPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [StringLength(maximumLength: 350)]
        public string? Description { get; set; }
    }
}