namespace Demo.BLL.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository  DepartmentRepository { get; set; }

        int Complete();
    }
}
