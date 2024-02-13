using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.BLL.Repositories;

public class GenericRepository<T>:IGenericRepository<T> where T : BaseEntity
{
    private protected readonly AppDBContext _dbContext; //NULL
    public GenericRepository(AppDBContext appDBContext) //Ask CLR To create object from DBContext
    {
        //_dbContext=new AppDBContext();
        _dbContext = appDBContext;
    }
    public void Add(T Entity)
        =>_dbContext.Add(Entity);
    //_dbContext.Set<T>().Add(Entity);


    public void Update(T Entity)   
        =>_dbContext.Update(Entity);
    

    public void Delete(T Entity)
        =>_dbContext.Remove(Entity);
    

    public T Get(int id)
    {
        //var department= _dbContext.Departments.Local.Where(D => D.Id == id).FirstOrDefault();
        //if(department == null)
        //    department= _dbContext.Departments.Where(D => D.Id == id).FirstOrDefault();
        //return department;

        //return _dbContext.Departments.Find(id);

        return _dbContext.Find<T>(id);
    }

    public IEnumerable<T> GetAll()
   => _dbContext.Set<T>().AsNoTracking().ToList();
}
