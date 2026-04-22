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
using System;

namespace Api.BLL.Services
{
    public class StockBaseService : IStockBaseService
    {
        private readonly UnitOfWorkLeaveSystem unitOfWork;
        private readonly IEventBus eventBus;

        public StockBaseService(UnitOfWorkLeaveSystem unitOfWork, IEventBus eventBus)
        {
            this.unitOfWork = unitOfWork;
            this.eventBus = eventBus;
        }

        public StockBaseDTO GetStockBaseById(string Id)
        {
            var r = this.unitOfWork.GetRepository<StockBase>().ExGetById(Id);
            var result = MapperProvider.Instance.Map<StockBaseDTO>(r);
            return result;
        }

        public DataTableWrapper<StockBaseDTO> GetDataTableRequest(GetStockBaseDataTableQuery query)
        {
            var r = this.unitOfWork.GetRepository<StockBase>().ExGetDataTableResult(query);
            var result = MapperProvider.Instance.Map<DataTableWrapper<StockBaseDTO>>(r);
            return result;
        }

        public string CreateStockBase(CreateStockBaseCommand cmd)
        {
            StockBase entity = this.GetCreateStockBase(cmd);
            this.unitOfWork.GetRepository<StockBase>().Add(entity);
            this.unitOfWork.Save();
            return entity.code;
        }

        public string EditStockBase(EditStockBaseCommand cmd)
        {
            StockBase entity = this.unitOfWork.GetRepository<StockBase>().ExGetById(cmd.code);
            entity = this.GetEditStockBase(cmd, entity);
            this.unitOfWork.Save();
            return entity.code;
        }

        public string DeleteStockBase(DeleteStockBaseCommand cmd)
        {
            StockBase entity = unitOfWork.GetRepository<StockBase>().ExGetById(cmd.Id);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"Could not find StockBase with Id {cmd.Id}");
            }
            this.unitOfWork.GetRepository<StockBase>().Delete(entity);
            this.unitOfWork.Save();
            return cmd.Id;
        }
    }

    public static class IStockBaseExtension
    {
        public static StockBase GetCreateStockBase(this IStockBaseService iStockBaseService, CreateStockBaseCommand cmd)
        {
            StockBase entity = new StockBase
            {
                code = cmd.code,
                name = cmd.name,
                isfavorite = cmd.isfavorite,
                isexclude = cmd.isexclude
            };
            return entity;
        }

        public static StockBase GetEditStockBase(this IStockBaseService iStockBaseService, EditStockBaseCommand cmd, StockBase entity)
        {
            entity.name = cmd.name;
            entity.isfavorite = cmd.isfavorite;
            entity.isexclude = cmd.isexclude;
            return entity;
        }
    }
}
