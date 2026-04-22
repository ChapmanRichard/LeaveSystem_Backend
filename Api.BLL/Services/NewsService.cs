using Api.Common.DataTable;
using Api.Common.Events;
using Api.Common.Providers;
using Api.Contract.Commands;
using Api.Contract.DTO;
using Api.Contract.Model;
using Api.Contract.Queries;
using Api.Contract.ServiceInterfaces;
using Api.DAL;
using Api.DAL.Repositories;

namespace Api.BLL.Services
{
    /// <summary>
    ///  
    /// </summary>
    public class NewsService : INewsService
    {
        private readonly UnitOfWorkLeaveSystem unitOfWork;
        private readonly IEventBus eventBus;

        public NewsService(UnitOfWorkLeaveSystem unitOfWork, IEventBus eventBus)
        {
            this.unitOfWork = unitOfWork;
            this.eventBus = eventBus;
        }

        #region 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NewsDTO GetNewsById(int Id)
        {
            var r = this.unitOfWork.GetRepository<News>().ExGetById(Id);
            var result = MapperProvider.Instance.Map<NewsDTO>(r);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTableWrapper<NewsDTO> GetDataTableRequest(GetNewsDataTableQuery query)
        {
            var r = this.unitOfWork.GetRepository<News>().ExGetDataTableResult(query);
            var result = MapperProvider.Instance.Map<DataTableWrapper<NewsDTO>>(r);
            return result;
        }

        #region Auto Query Code
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public int CreateNews(CreateNewsCommand cmd)
        {
            News entity = this.GetCreateNews(cmd);
            this.unitOfWork.GetRepository<News>().Add(entity);
            this.unitOfWork.Save();
            return entity.NewsId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public int EditNews(EditNewsCommand cmd)
        {
            News entity = this.unitOfWork.GetRepository<News>().ExGetById(cmd.NewsId);
            entity = this.GetEditNews(cmd, entity);
            this.unitOfWork.Save();
            return entity.NewsId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int DeleteNews(DeleteNewsCommand cmd)
        {
            News? entity = unitOfWork.GetRepository<News>().ExGetById(cmd.Id);
            if (entity == null)
            {
                // ���Ը���ҵ�������׳��쳣�򷵻�����ֵ
                throw new ArgumentNullException(nameof(entity), $"δ�ҵ�IdΪ{cmd.Id}�����ż�¼��");
                // ���� return -1; ��ʾɾ��ʧ��
            }
            this.unitOfWork.GetRepository<News>().Delete(entity);
            this.unitOfWork.Save();
            return cmd.Id;
        }
        #region Auto Comman Code
        #endregion

        #endregion

    }
    public static class INewsExtension
    {
        public static News GetCreateNews(this INewsService iNewsService, CreateNewsCommand cmd)
        {
            News entity = new News();

            if (cmd.NewsDate != default)
                entity.NewsDate = cmd.NewsDate;
            if (cmd.Content != default)
                entity.Content = cmd.Content;
            if (cmd.Status != default)
                entity.Status = cmd.Status;

            return entity;
        }
        public static News GetEditNews(this INewsService iNewsService, EditNewsCommand cmd, News entity)
        {

            if (cmd.NewsDate != default)
                entity.NewsDate = cmd.NewsDate;
            if (cmd.Content != default)
                entity.Content = cmd.Content;
            if (cmd.Status != default)
                entity.Status = cmd.Status;

            return entity;
        }
    }
}
