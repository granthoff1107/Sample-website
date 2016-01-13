using Flowbandit.Models;
using FlowRepository;
using FlowRepository.ExendedModels.Contracts;
using FlowRepository.ExtendedModels.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flowbandit.Controllers.Rules
{
    public class CommentRule
    {
        public static void Comment<T>(IRepository _repository, T NewComment)
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