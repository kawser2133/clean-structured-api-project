using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Entities.Business
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public decimal TotalBill { get; set; }
        public int TotalQuantity { get; set; }
        public DateTime ProcessingData { get; set; }
        [StringLength(maximumLength: 350)]
        public string? Description { get; set; }

        public List<OrderDetailsViewModel>? OrderDetails { get; set; }
    }
}
