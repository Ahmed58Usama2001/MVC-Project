using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.BLL.Repositories;

public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{

    public EmployeeRepository(AppDBContext dBContext) //Ask CLR For creating object from db context
        :base(dBContext)
    {
    }

    public IQueryable<Employee> GetEmployeesByAddress(string address)
    {
        return _dbContext.Employees.Where(E=>E.Address.ToLower().Contains(address.ToLower()));
    }

    public IQueryable<Employee> SearchByName(string name)
          =>_dbContext.Employees.Where(E=>E.Name.ToLower().Contains(name.ToLower()));

    void IEmployeeRepository.Detach(Employee employee)
    {
        var entry = _dbContext.Entry(employee);

        if (entry.State != EntityState.Detached)
        {
            entry.State = EntityState.Detached;
        }
    }
}

