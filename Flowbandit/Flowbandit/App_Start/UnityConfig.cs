using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.FlowRepository;
using FlowRepository.Repositories.Models.FlowLog;
using FlowRepository.Data.Rules;
using System.Web.Configuration;

namespace Flowbandit
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterInstance<EmailSender>(GetEmailSender());


            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IPostRepository, PostRepository>();
            container.RegisterType<ITagRepository, TagRepository>();
            container.RegisterType<IVideoRepository, VideoRepository>();
            container.RegisterType<IFlowLogRepository, LogRepository>();

            

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        static EmailSender GetEmailSender()
        {
            var smtpAddress = WebConfigurationManager.AppSettings["smtpServer"];
            var smtpUsername = WebConfigurationManager.AppSettings["smtpUsername"];
            var smtpPassword = WebConfigurationManager.AppSettings["smtpPassword"];
            return new EmailSender(smtpAddress, smtpUsername, smtpPassword);
        }
    }
}