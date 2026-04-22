using Api.Common.Events;
using Api.Contract.Constants;
using Api.Contract.Events;
using Api.Contract.ServiceInterfaces;
using Api.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.BLL.EventHandlers
{
    public class NewsEventHandler : IEventHandler<NewsEvent>
    {
        private readonly UnitOfWorkLeaveSystem unitOfWork;
        public NewsEventHandler(UnitOfWorkLeaveSystem unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public void Handle(NewsEvent @event)
        {

           
        }
    }
}
