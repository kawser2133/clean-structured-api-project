using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Entities.General
{
    //Customer Table added for correlational things sharing
    [Table("Customers")]
    public class Customer : Base<int>
    {
        [Required, StringLength(maximumLength: 100, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;
        [Required, DataType(DataType.EmailAddress), StringLength(maximumLength: 100, MinimumLength = 5)]
        public string Email { get; set; } = string.Empty;
        public decimal? Balance { get; set; } = 0;
    }
}
