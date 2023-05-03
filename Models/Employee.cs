using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Employee
    {
        public Guid Id { get; set; }

        public String? Name { get; set; }

        public String? City { get; set; }
        
    }
}