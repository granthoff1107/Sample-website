using FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models
{
    public class PostVM : BaseModel<IPostRepository>
    {
        /// <summary>
        /// Includes comments and tags
        /// </summary>
        public Post CurrentPost;

        #region Comment Properties

        protected List<IComment> _comments;
        public List<IComment> Comments
        {
            get
            {
                if (_comments == null)
                {
                    if (CurrentPost != null)
                    {
                        _comments = CurrentPost.PostComments.Where(p => p.FK_ParentID == null)
                                                        .Select(c => new PostCommentDisplay(c)).ToList<IComment>();
                    }
                    else
                    {
                        _comments = new List<IComment>();
                    }
                }

                return _comments;
            }
        }

        //protected new IPostRepository Data
        //{
        //    get
        //    {
        //        return (base.Data as IPostRepository);
        //    }
        //}

        #endregion // Comment Properties

        #region Post Properties

        public int PostID
        {
            get
            {
                if (CurrentPost != null)
                {
                    return CurrentPost.ID;
                }
                return default(int);
            }
        }

        public string PostName
        {
            get
            {
                if (CurrentPost != null)
                {
                    return CurrentPost.Title;
                }
                return string.Empty;
            }
        }

        public string Author
        {
            get
            {
                if (CurrentPost != null)
                {
                    return CurrentPost.User.Username;
                }
                return string.Empty;
            }
        }

        public int AuthorID
        {
            get
            {
                if (CurrentPost != null && CurrentPost.User != null)
                {
                    return CurrentPost.User.ID;
                }
                return default(int);
            }
        }

        public string Entry
        {
            get
            {
                if (CurrentPost != null)
                {
                    return CurrentPost.Entry;
                }
                return string.Empty;
            }
        }

        public string Created
        {
            get
            {
                //TODO: Refactor this logic into an extension method
                return (CurrentPost.Created == default(DateTime) ? DateTime.Now : CurrentPost.Created).ToString();
            }
        }

        public string VirtualPhotoCoverPath
        {
            get
            {
                return (CurrentPost != null && false == string.IsNullOrWhiteSpace(CurrentPost.CoverPhotoUrl)) ? @"~\" + CurrentPost.CoverPhotoUrl : string.Empty;
            }
        }

        public bool isVisible
        {
            get
            {
                return (CurrentPost != null && CurrentPost.ID != 0) ? CurrentPost.Visible : true;
            }
        }

        #endregion //Post Properties

        public PostVM(IPostRepository Data, int ID)
            : base(Data)
        {
            CurrentPost = Data.VisiblePostByIDWithCommentsTagsUsers(ID);
        }

        public PostVM()
            : base(null)
        {
            CurrentPost = new Post();
        }
    }
}