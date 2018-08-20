using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

[assembly: OwinStartup(typeof(Api.Startup))]

namespace Api
{
    public class Startup
    {
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config = Register(config);

            SignalRConfiguration(app);

            // WebApi hosting configurations /w Logger Configuration
            app.UseWebApi(config);

        }

        internal HttpConfiguration Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            var constraintResolver = new System.Web.Http.Routing.DefaultInlineConstraintResolver();
            //constraintResolver.ConstraintMap.Add("email", typeof(EmailRouteConstraint));
            config.MapHttpAttributeRoutes(constraintResolver);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}", 
                defaults: new { id = RouteParameter.Optional }
            );

            // Web API configuration and services
            //config.EnableCors(new CustomCorsPolicyProvider());
            //SetCors(config, options);

            //config.SuppressDefaultHostAuthentication();

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Remove(config.Formatters.FormUrlEncodedFormatter);

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));

            // Adding F/W Action Filter for Logging Request and Response
            //AddFilters(config, options);

            return config;
        }

        public void SignalRConfiguration(IAppBuilder app)
        {
            // Branch the pipeline here for requests that start with "/signalr"
            //app.Map("http://localhost:51197/signalr", map =>
            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);

                if (ConfigurationManager.AppSettings.AllKeys.Contains("TransportConnectTimeout"))
                    GlobalHost.Configuration.TransportConnectTimeout = TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings["TransportConnectTimeout"]));

                if (ConfigurationManager.AppSettings.AllKeys.Contains("ConnectionTimeout"))
                    GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings["ConnectionTimeout"]));

                GlobalHost.Configuration.KeepAlive = null;
                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    EnableJSONP = true,
                    EnableJavaScriptProxies = true,
                    EnableDetailedErrors = true

                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}