using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.DAL.Models;

public class Employee : BaseEntity
{
    public string Name { get; set; }

    public int Age { get; set; }


    public string Address { get; set; }

    public decimal Salary { get; set; }

    public bool IsActive { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public DateTime HireDate { get; set; }

    public bool IsDelete { get; set; } = false;

    public DateTime CreationDate { get; set; }=DateTime.Now;

    public string ImageName { get; set; }

    //[ForeignKey("Department")]
    public int DepartmentId { get; set; } //Foreign key column

    //[InverseProperty("Employees")]
    //Navigational Property [One]
    public Department? Department { get; set; }

}
