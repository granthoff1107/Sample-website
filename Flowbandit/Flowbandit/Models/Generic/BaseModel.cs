using Flowbandit.Controllers;
using FlowRepository;
using FlowRepository.Repositories.Contracts.FlowRepository;
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
    }

    public class BaseModel
    {
        public const string WEBSITENAME = "Flow Bandit";

        public int CurrentUser
        {
            get
            {
                return LoginHelper.UserID;
            }
        }

        public string isAnon
        {
            get
            {
                return LoginHelper.isAnon.ToString().ToLower();
            }
        }

        public BaseModel()
        {
        }

        protected int GetTotalPageCountFromItems(int tmpCount, int resultsPerPage)
        {
            int totalPages = 0;
            //make sure there are results and avoid divide by 0
            if (tmpCount > 0 && resultsPerPage > 0)
            {
                totalPages = tmpCount / resultsPerPage;

                // if there is a remainder then there is another page
                if (tmpCount % resultsPerPage != 0)
                {
                    totalPages++;
                }
            }

            return totalPages;
        }
    }
}