using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace northwindAPI.Data.ModelDTO
{
    public class EmployeeDTO
    {
         public int EmployeeId { get; set; }

        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;
        public string? HomePhone {get; set;}
        public DateTime HireDate {get; set;}

    }
}