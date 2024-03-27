using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace northwindAPI.Data.ModelDTO
{
    public class RegionDTO
    {
         public int RegionId { get; set; }

         public string RegionDescription { get; set; } = null!;
    }
}