using Flowbandit.Controllers;
using FlowRepository;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowService.DTOs.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models.Generic
{
    public class BaseModel<TRepository> : BaseModel
        where TRepository : class, IFlowRepository
    {
        public TRepository DataRepository;

        public BaseModel(TRepository repository)
        {
            this.DataRepository = repository;
        }

        public int CurrentUser
        {
            get
            {
                return LoginHelper.UserID;
            }
        }
    }
}