using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace northwindAPI.Data.ModelDTO
{
    public  class OrderDetailDTO
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public short Quantity { get; set; }

    public float Discount { get; set; }

}
}