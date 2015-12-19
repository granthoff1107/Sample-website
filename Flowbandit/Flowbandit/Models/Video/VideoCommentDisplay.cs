using FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models
{
    public class VideoCommentDisplay : IComment
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
                return (Comment.FK_UserID == null);
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
                return Comment.VideoComment1.Count > 0;
            }
        }

        public List<IComment> Children
        {
            get
            {

                if (_children == null || hasChildren)
                {
                    _children = Comment.VideoComment1.Select(c => new VideoCommentDisplay(c)).ToList<IComment>() ?? new List<VideoCommentDisplay>().ToList<IComment>();
                }

                return _children;
            }
        }

        protected List<IComment> _children;

        protected VideoComment Comment;

        public VideoCommentDisplay(VideoComment Comment)
        {
            this.Comment = Comment;
        }

    }
}