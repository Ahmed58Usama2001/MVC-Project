using Demo.DAL.Models;

namespace Demo.BLL.Interfaces;

public interface IGenericRepository<T>where T : BaseEntity
{
    IEnumerable<T> GetAll();
    T Get(int id);

    void Add(T Entity);

    void Update(T Entity);

    void Delete(T Entity);
}
