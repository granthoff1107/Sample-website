﻿using FlowRepository;
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
        public IEnumerable<VideoVM> RecentVideos;
        public IEnumerable<PostVM> RecentPosts;

        public string THANKS_FOR_REGISTERING_MESSAGE = "Thanks for registering.  Login and Tell us more about yourself";

        public bool IsInitialLogin = false;

        public ProfileDTO(IUserRepository userRepository, int id, bool isInitialLogin)
            : base(userRepository)
        {
            IsInitialLogin = isInitialLogin;

            //Do no worry about disposing, the context is passed during conversion so the controller will handle disposal
            var videos = userRepository.ConvertToRepository<VideoRepository>().GetMostRecentVideos(0, GlobalInfo.RESULTSPERPAGE, CurrentUser, id);
            var posts = userRepository.ConvertToRepository<PostRepository>().GetMostRecentPosts(0, GlobalInfo.RESULTSPERPAGE, CurrentUser, id);

            foreach (var post in posts)
            {
                post.Content.Entry = post.Content.Entry.Substring(0, Math.Min(post.Content.Entry.Length, GlobalInfo.DISPLAY_TEXT_MAX_LENGTH)) + "...";
            }

            //foreach (var video in videos)
            //{
            //    video.Content.Entry = video.Content.Entry.Substring(0, Math.Min(video.Content.Entry.Length, GlobalInfo.DISPLAY_TEXT_MAX_LENGTH)) + "...";
            //}

            RecentPosts = posts.Select(post => new PostVM(post));
            RecentVideos = videos.Select(video => new VideoVM(video));

        }
    }
}