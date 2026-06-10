using Api.Common.Repositories;
using Api.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Api.DAL
{
    public class UnitOfWorkLeaveSystem : UnitOfWorkBase
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkLeaveSystem(LeaveSystemDBContext dbContext, IServiceProvider serviceProvider)
        {
            this._dbContext = dbContext;
            this._serviceProvider = serviceProvider;
        }

        /// <summary>
        /// ��̬��ȡָ��ʵ�����͵Ĳִ�ʵ����
        /// </summary>
        /// <typeparam name="T">ʵ�����ͣ�������һ�� class��</typeparam>
        /// <returns>��ʵ���Ӧ�� LeaveSystemDBRepository ʵ����</returns>
        public LeaveSystemDBRepository<T> GetRepository<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<LeaveSystemDBRepository<T>>();
        }
    }
}