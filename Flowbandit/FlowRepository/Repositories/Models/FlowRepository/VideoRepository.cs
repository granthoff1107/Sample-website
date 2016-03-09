using FlowRepository.Data.Rules;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Models.FlowRepository
{
    public class VideoRepository : ContentRepository, IVideoRepository
    {
        #region Constructors

        public VideoRepository()
            : base()
        {
        }

        public VideoRepository(FlowCollectionEntities context)
            : base(context)
        {
        } 

        #endregion

        #region IVideoRepostory Members

        public List<Video> GetMostRecentVideos(int pageNumber, int resultsPerPage, int currentUser = 0, int? userId = null, bool shouldStripTags = true)
        {
            return this.GetMostRecentVisibleContent(All<Video>(), pageNumber, resultsPerPage, currentUser, userId, shouldStripTags);
        }

        public Video FindVisibleVideoWithCommentsTagsUser(int id, int currentUser = 0)
        {
            return GetVisibleContentByIdWithCommentsTagsUsers<Video>(id, currentUser);
        }

        public void EditVideo(Video video, List<int> tagIds)
        {
            this.Edit<Video>(video, tagIds);
        }

        public List<Video> SearchTitles(string[] searchTerms, int pageNumber, int resultsPerPage, int currentUser = 0)
        {
            return this.SearchContent<Video>(searchTerms, pageNumber, resultsPerPage, currentUser);
        }

        #endregion
    }
}
