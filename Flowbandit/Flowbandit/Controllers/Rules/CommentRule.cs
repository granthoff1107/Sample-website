using Flowbandit.Models;
using FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.ExtendedModels.Contracts;

namespace Flowbandit.Controllers.Rules
{
    public class CommentRule
    {
        public static void Comment<T>(IFlowRepository _repository, T NewComment)
           where T : class, IHasCommentProperties
        {
            if (!GlobalInfo.IsAnon)
            {
                NewComment.FK_UserID = GlobalInfo.User.UserID;
            }

            NewComment.Created = DateTime.Now;
            _repository.Add<T>(NewComment);
            _repository.SaveChanges();
        }
    }
}