using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace northwindAPI.Data.ModelDTO
{
    public class TerritoryDTO
    {
         public string TerritoryId { get; set; } = null!;

         public string TerritoryDescription { get; set; } = null!;

         public int RegionId { get; set; }
    }
}