using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infinite.MVCCore.Taxibooking.Models
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
    }
}
