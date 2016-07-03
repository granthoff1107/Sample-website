using Flowbandit.Models;
using Flowbandit.Models.CustomActionResult;
using Flowbandit.Models.Authorization;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowService.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flowbandit.Controllers
{
    public class MessagesController :  BaseController<IMessageRepository>
    {
        public MessagesController(IMessageRepository repository, IFlowLogRepository logRepository)
            : base(repository, logRepository)
        {
        }

        [HttpGet]
        [FBAuthorizeLevel(MaximumLevel = BASIC_LEVEL)]
        public JsonNetResult GetUnreadMessages()
        {
            var service = new MessageService(this._repository);
            var messageDTOs = service.GetUnreadMessages(GlobalInfo.UserId.Value);
            return new JsonNetResult {
                        Data = messageDTOs,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [FBAuthorizeLevel(MaximumLevel = BASIC_LEVEL)]
        public JsonNetResult GetConversationMessages(int id)
        {
            var service = new MessageService(this._repository);
            var messageDTOs = service.GetConversationMessages(GlobalInfo.UserId.Value, id);
            return  new JsonNetResult {
                Data = messageDTOs,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}