using FlowRepository;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowService.DTOs.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowService.DTOs.Posts
{
    public class PostDTO : ContentBaseModel<Post>
    {
        #region Constructors

        public PostDTO(Post post) : base(post)
        {
        }

        #endregion

        #region Post Properties

        public int PostId
        {
            get
            {
                if (_parentContent != null)
                {
                    return _parentContent.Id;
                }
                return default(int);
            }
        }

        public string VirtualPhotoCoverPath
        {
            get
            {
                return (_parentContent != null 
                        && false == string.IsNullOrWhiteSpace(_parentContent.CoverPhotoUrl)) 
                            ? @"~\" + _parentContent.CoverPhotoUrl : string.Empty;
            }
        }

        #endregion //Post Properties
    }
}