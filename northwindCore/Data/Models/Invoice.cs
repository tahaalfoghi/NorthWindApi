using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace northwindAPI.Data.View
{
    public class Invoice
    {
       public int OrderId { get; set; }
       public string? CustomerId { get; set; }
       public string? ContactName { get; set; }
       public int ProductId { get; set; }
       public string? ProductName { get; set; }
       public decimal UnitPrice { get; set; }
       public short Quantity { get; set; }
       public float ExtendedPrice { get; set; }
       public decimal Total { get; set; }
       public DateTime OrderDate { get; set; }
        
    }
}