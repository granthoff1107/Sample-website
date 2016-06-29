using FlowRepository;
using FlowRepository.ExtendedModels.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.DTOs.Generic
{
    public class ContentBaseModel<T> : BaseModel
        where T : IHasContent
    {
        #region Members

        protected T _parentContent;

        #endregion

        #region Constructors

        public ContentBaseModel(T entity)
            : base()
        {
            this._parentContent = entity;
        }

        public ContentBaseModel()
            : base()
        {
        }

        #endregion
        
        public List<IComment> Comments
        {
            get
            {
                return _parentContent.Content.ContentComments.Where(p => p.ParentCommentId == null)
                                                     .Select(c => new CommentDisplay(c))
                                                     .ToList<IComment>();
            }
        }

        //TODO change this to be an ITag to removed database Dependencies
        public List<Tag> Tags
        {
            get
            {
                return _parentContent.Content.TagsToContents.Select(x => x.Tag).ToList();
            }
        }

        public int ContentId
        {
            get
            {
                return _parentContent.Content.Id;
            }
        }

        public string Title
        {
            get
            {
                return _parentContent.Content.Title;
            }
        }

        public string Owner
        {
            get
            {
                return _parentContent.Content.User.Username;
            }
        }

        public int OwnerId
        {
            get
            {
                return _parentContent.Content.User.ID;
            }
        }

        public string Entry
        {
            get
            {
                return _parentContent.Content.Entry;
            }
        }

        public string Created
        {
            get
            {
                //TODO: Refactor this logic into an extension method
                return (_parentContent.Content.Created == default(DateTime) ? DateTime.Now : _parentContent.Content.Created).ToString();
            }
        }

        public bool IsVisible
        {
            get
            {
                return (ContentId != default(int)) ? _parentContent.Content.Visible : true;
            }
        }
    }
}
