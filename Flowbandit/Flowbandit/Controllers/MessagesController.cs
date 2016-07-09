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
using FlowService.DTOs.Messages;
using FlowRepository;

namespace Flowbandit.Controllers
{
    //TODO: Move Services To IOC Container for all Controllers
    public class MessagesController : BaseController<IMessageRepository>
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

        [HttpPost]
        [FBAuthorizeLevel(MaximumLevel = BASIC_LEVEL)]
        //TODO: Don't use take in message you should take 
        //in a DTO so your models are dependent on your database
        public JsonNetResult AddMessage(Message message)
        {
            var service = new MessageService(this._repository);
            if (ModelState.IsValid)
            {
                service.AddMessage(message, GlobalInfo.UserId.Value);
                var messageDTOs = service.GetConversationMessages(GlobalInfo.UserId.Value, message.ReceiverUserId);
                
                return new JsonNetResult
                {
                    Data = messageDTOs,
                };
            }
            
            //TODO: This is shit find a better way to return response codes
            Response.StatusCode = 400;
            return new JsonNetResult();
        }
    }
}