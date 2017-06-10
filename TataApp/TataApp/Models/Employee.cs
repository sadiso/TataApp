using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TataApp.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        
        public string FirstName { get; set; }

        
        public string LastName { get; set; }

        
        public int EmployeeCode { get; set; }

        
        public int DocumentTypeId { get; set; }

        
        public int LoginTypeId { get; set; }

        
        public string Document { get; set; }

        
        public string Picture { get; set; }

        
        public string Email { get; set; }

        
        public string Phone { get; set; }

        
        public string Address { get; set; }

        
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }
        
    }
}
