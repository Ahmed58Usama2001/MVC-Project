using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.DAL.Models;

public class Department:BaseEntity
{

    [Required(ErrorMessage ="Code is Required")]
    public string  Code { get; set; }

    [Required(ErrorMessage = "Name is Required")]
    public string  Name { get; set; }

    [Display(Name ="Date Of Creation")]
    public DateTime DateOfCreation { get; set; }

    //[InverseProperty(nameof(Employee.Department))]
    //Navigational property many
    //public ICollection<Employee> Employees { get; set; }=new HashSet<Employee>();
}
