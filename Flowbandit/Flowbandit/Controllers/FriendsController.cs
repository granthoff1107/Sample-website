using Flowbandit.Models;
using Flowbandit.Models.Authorization;
using Flowbandit.Models.CustomActionResult;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flowbandit.Controllers
{
    public class FriendsController : BaseController<IFriendRepository>
    {
        #region Constructors

        public FriendsController(IFriendRepository repository, IFlowLogRepository logRepository)
            : base(repository, logRepository)
        {
        }

        #endregion

        [HttpGet]
        [FBAuthorizeLevel(MaximumLevel = BASIC_LEVEL)]
        public JsonNetResult GetAllFriends()
        {
            var service = new FriendService(this._repository);
            var friendDTOs = service.GetAllFriends(GlobalInfo.UserId.Value);

            return new JsonNetResult
            {
                Data = friendDTOs,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}