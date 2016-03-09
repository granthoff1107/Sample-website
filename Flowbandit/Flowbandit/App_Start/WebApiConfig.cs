using FlowRepository;
using Microsoft.Data.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace Flowbandit
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ODataConventionModelBuilder builder = GetPopulatedBuilder();

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "Odata",
                model: builder.GetEdmModel());


            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            //config.EnableSystemDiagnosticsTracing();
        }

        public static ODataConventionModelBuilder GetPopulatedBuilder()
        {
            var builder = new ODataConventionModelBuilder();

            var tagsToContentEntity = builder.EntitySet<TagsToContent>("TagsToContents");
            tagsToContentEntity.EntityType.HasKey(x => x.ContentId);
            tagsToContentEntity.EntityType.HasKey(x => x.TagId);

            var postEntity = builder.EntitySet<Post>("Posts");
            var videoEntity = builder.EntitySet<Video>("Videos");

            builder.EntitySet<Tag>("Tags");
            return builder;
        }
        
    }
}
