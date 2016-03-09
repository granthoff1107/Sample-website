using FlowRepository;
using FlowRepository.ExtendedModels.Contracts;
using FlowRepository.Repositories.Contracts.FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models.Generic
{
    public class ContentBaseModel<TRepository, TContent> : BaseModel<TRepository>
        where TRepository : class, IContentRepository
        where TContent : class, IHasContent, new()
    {
        protected TContent _contentParent;

        #region Constructors

        public ContentBaseModel(TRepository repository, int id)
            : base(repository)
        {
            _contentParent = repository.GetVisibleContentByIdWithCommentsTagsUsers<TContent>(id, CurrentUser);
        }

        public ContentBaseModel()
            : this(new TContent())
        {
        }

        public ContentBaseModel(TContent content) : base(null)
        {
            _contentParent = content;
            if(_contentParent.Content == null)
            {
                _contentParent.Content = new Content();
            }
        }

        #endregion

        public List<IComment> Comments
        {
            get
            {
                return _contentParent.Content.ContentComments.Where(p => p.ParentCommentId == null)
                                                     .Select(c => new CommentDisplay(c))
                                                     .ToList<IComment>();
            }
        }

        //TODO change this to be an ITag to removed database Dependencies
        public List<Tag> Tags
        {
            get
            {
                return _contentParent.Content.TagsToContents.Select(x => x.Tag).ToList();
            }
        }

        public int ContentId
        {
            get
            {
                return _contentParent.Content.Id;
            }
        }

        public string Title
        {
            get
            {
                return _contentParent.Content.Title;   
            }
        }

        public string Owner
        {
            get
            {
                return _contentParent.Content.User.Username;
            }
        }

        public int OwnerId
        {
            get
            {
                return _contentParent.Content.User.ID;   
            }
        }

        public string Entry
        {
            get
            {
                return _contentParent.Content.Entry;
            }
        }

        public string Created
        {
            get
            {
                //TODO: Refactor this logic into an extension method
                return (_contentParent.Content.Created == default(DateTime) ? DateTime.Now : _contentParent.Content.Created).ToString();
            }
        }

        public bool isVisible
        {
            get
            {
                return (ContentId != default(int)) ? _contentParent.Content.Visible : true;
            }
        }
    }
}