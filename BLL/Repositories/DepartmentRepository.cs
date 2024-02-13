using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.BLL.Repositories;

public class DepartmentRepository : GenericRepository<Department>,IDepartmentRepository
{
    public DepartmentRepository(AppDBContext dBContext)
        :base(dBContext)
    {
        
    }

}
