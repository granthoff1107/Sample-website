using Flowbandit.Models.Generic;
using FlowRepository;
using FlowRepository.Repositories.Contracts.FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models
{
    public class PostVM : ContentBaseModel<IPostRepository, Post>
    {
        #region Post Properties

        public int PostId
        {
            get
            {
                if (_contentParent != null)
                {
                    return _contentParent.Id;
                }
                return default(int);
            }
        }

        public string VirtualPhotoCoverPath
        {
            get
            {
                return (_contentParent != null && false == string.IsNullOrWhiteSpace(_contentParent.CoverPhotoUrl)) ? @"~\" + _contentParent.CoverPhotoUrl : string.Empty;
            }
        }
        #endregion //Post Properties

        public PostVM(IPostRepository postRepository, int id)
            : base(postRepository, id)
        {
        }

        public PostVM(Post post)
            : base(post)
        {
        }

        public PostVM()
            : base()
        {
        }
    }
}