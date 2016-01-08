using FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models
{
    public class VideoVM : BaseModel
    {
        public Video CurrentVideo;

        public int VideoID
        {
            get
            {
                return CurrentVideo != null ? CurrentVideo.ID : default(int);
            }
        }

        public string VideoName
        {
            get
            {
                return CurrentVideo != null ? CurrentVideo.Name : string.Empty;
            }
        }

        public string Poster
        {
            get
            {
                return CurrentVideo.User.Username;
            }
        }

        public string Created
        {
            get
            {
                //TODO: Refactor this logic into an extension method
                return (CurrentVideo.Created == DateTime.MinValue ? DateTime.Now : CurrentVideo.Created).ToString();
            }
        }

        public string VideoUrl
        {
            get
            {
                return CurrentVideo != null ? CurrentVideo.VideoUrl : string.Empty;
            }
        }

        public string ThumbnailUrl
        {
            get
            {
                return CurrentVideo != null ? CurrentVideo.ThumbnailUrl : string.Empty;
            }
        }

        public bool isVisible
        {
            get
            {
                return CurrentVideo != null ? CurrentVideo.Visible : true;
            }
        }

        public string Description
        {
            get
            {
                return CurrentVideo != null ? CurrentVideo.Description : string.Empty;
            }
        }


        protected List<IComment> _comments;
        public List<IComment> Comments
        {
            get
            {
                if (_comments == null)
                {
                    _comments = CurrentVideo.VideoComments.Where(p => p.FK_ParentID == null)
                                                    .Select(c => new VideoCommentDisplay(c)).ToList<IComment>();
                }

                return _comments;
            }
        }


        public VideoVM(IVideoRepository Data, int ID)
            : base(Data)
        {
            CurrentVideo = Data.FindVisibleVideoWithCommentsTagsUser(ID);
        }

        public VideoVM()
            : base(null)
        {
            CurrentVideo = new Video();
        }
    }

}