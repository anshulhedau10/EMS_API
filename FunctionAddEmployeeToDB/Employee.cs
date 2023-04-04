using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAddEmployeeToDB
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public long MobileNo { get; set; }
        public int DepartmentId { get; set; }
    }
}
