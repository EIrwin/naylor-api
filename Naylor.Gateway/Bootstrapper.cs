using Nancy;
using Nancy.Bootstrapper;
using Nancy.Hosting.Self;
using Nancy.TinyIoc;
using Naylor.Data;
using Naylor.Services;
using Naylor.Services.Meetings;
using Naylor.Services.Users;

namespace Naylor.Gateway
{
    public class Bootstrapper:DefaultNancyBootstrapper
    {
        private TinyIoCContainer _container;
        private string _mongoEndpoint;

        public Bootstrapper(string mongoEndpoint)
        {
            _mongoEndpoint = mongoEndpoint;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            _container = container;

            var configuration = new HostConfiguration() { UrlReservations = { CreateAutomatically = true } };
            container.Register(configuration);

            //register dependencies

            IRepository<User> userRepository = new UserRepository(_mongoEndpoint);
            IRepository<Meeting> meetingRepository = new MeetingRepository(_mongoEndpoint);

            IUserService userService = new UserService(userRepository);
            IMeetingService meetingService = new MeetingService(meetingRepository,userService);

            container.Register(userService);
            container.Register(meetingService);

            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,DELETE,OPTIONS");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Accept, Origin, Content-type,Authorization");
            };

            base.ApplicationStartup(container, pipelines);
        }
    }
}
