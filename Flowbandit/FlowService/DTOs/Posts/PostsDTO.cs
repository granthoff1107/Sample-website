using System.Collections.Generic;
using System.Linq;
using FlowRepository.Models.Pagination;
using FlowService.DTOs.Generic;
using FlowRepository;
using System;

namespace FlowService.DTOs.Posts
{
    public class PostsDTO : ContentsBaseModel<Post, PostDTO>
    {
        #region Constructors

        public PostsDTO(IEnumerable<Post> posts, PaginationInfo pageInfo) : base(posts, pageInfo)
        {
        }

        #endregion

        protected override Func<Post, PostDTO> _createEntity
        {
            get
            {
                return x => new PostDTO(x);
            }
        }
    }
}