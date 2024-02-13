using Demo.DAL.Models;

namespace Demo.BLL.Interfaces;

public interface IEmployeeRepository : IGenericRepository<Employee> 
{
    IQueryable<Employee> GetEmployeesByAddress(string address);

    IQueryable<Employee> SearchByName(string name);

    public void Detach(Employee employee);

}
