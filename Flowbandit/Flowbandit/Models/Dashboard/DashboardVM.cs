using FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models
{
    public class DashboardVM : BaseModel
    {
        public DashboardVM(IRepository Data)
            : base(Data)
        {

        }
    }
}