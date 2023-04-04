using System.ComponentModel.DataAnnotations;

namespace EmsApi.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [RegularExpression(@"^\d{10}$")]
        public long MobileNo { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
    }
}
