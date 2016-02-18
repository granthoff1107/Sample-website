using FlowRepository;
using FlowRepository.Data.Rules;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models.Accounts
{
    public class ProfileDTO : BaseModel<IUserRepository>
    {
        public List<Video> RecentVideos;
        public List<Post> RecentPosts;

        public string THANKS_FOR_REGISTERING_MESSAGE = "Thanks for registering.  Login and Tell us more about yourself";

        public bool IsInitialLogin = false;

        public ProfileDTO(IUserRepository userRepository, int id, bool isInitialLogin)
            : base(userRepository)
        {
            IsInitialLogin = isInitialLogin;

            //Do no worry about disposing, the context is passed during conversion so the controller will handle disposal
            RecentVideos = userRepository.ConvertToRepository<VideoRepository>().GetMostRecentVideos(0, GlobalInfo.RESULTSPERPAGE, id);
            RecentPosts = userRepository.ConvertToRepository<PostRepository>().GetMostRecentPosts(0, GlobalInfo.RESULTSPERPAGE, id);

            foreach (var post in RecentPosts)
            {
                post.Entry = post.Entry.Substring(0, Math.Min(post.Entry.Length, GlobalInfo.DISPLAY_TEXT_MAX_LENGTH)) + "...";
            }
        }
    }
}