using Api.Common.Repositories;
using Api.DAL;

namespace Api.DAL.Repositories
{
    public class LeaveSystemDBRepository<T> : RepositoryBase<T>, IRepositoryBase<T> where T : class
    {
        public LeaveSystemDBRepository(LeaveSystemDBContext dbcontext) : base(dbcontext)
        {

        }

    }
}
