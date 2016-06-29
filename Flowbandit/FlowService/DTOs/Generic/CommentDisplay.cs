using FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowService.DTOs.Generic
{
    public class CommentDisplay : IComment
    {
        public string Username
        {
            get
            {
                if (AnonUser)
                {
                    return "Anonymous";
                }
                return Comment.User.Username;
            }
        }

        public bool AnonUser
        {
            get
            {
                return (Comment.UserId == null);
            }
        }

        public string CommentText
        {
            get
            {
                return Comment.Comment;
            }
        }

        public string Created
        {
            get
            {
                return Comment.Created.ToString();
            }
        }

        string _ProfilePicUrl;
       
        public string UserProfilePicUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_ProfilePicUrl))
                {
                    _ProfilePicUrl = !AnonUser && !string.IsNullOrEmpty(Comment.User.ImageUrl) ? Comment.User.ImageUrl : "http://placehold.it/64x64";
                }

                return _ProfilePicUrl;
            }
        }

        public bool hasChildren
        {
            get
            {
                return Comment.ContentComments1.Count > 0;
            }
        }

        public List<IComment> Children
        {
            get
            {
                if (_children == null || hasChildren)
                {
                    _children = Comment.ContentComments1.Select(c => new CommentDisplay(c)).ToList<IComment>() ?? new List<CommentDisplay>().ToList<IComment>();
                }

                return _children;
            }
        }

        protected List<IComment> _children;

        protected ContentComment Comment;

        public CommentDisplay(ContentComment Comment)
        {
            this.Comment = Comment;
        }

    }
}