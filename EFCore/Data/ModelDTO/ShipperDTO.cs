using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace northwindAPI.Data.ModelDTO
{
    public class ShipperDTO
    {
         public int ShipperId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? Phone { get; set; }
    }
}