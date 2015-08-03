using Autofac;
using Autofac.Integration.SignalR;
using ChatRoom.Data;
using ChatRoom.Data.AzureTableRepositories;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System.Reflection;

[assembly: OwinStartupAttribute(typeof(ChatRoom.Startup))]
namespace ChatRoom
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            var config = new HubConfiguration();

            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            builder.RegisterType<ChatMessageAzureTableRepository>().SingleInstance().As<IChatMessagePersister>();
            builder.RegisterType<UserAzureTableRepository>().SingleInstance().As<IUserPersister>();

            var container = builder.Build();

            GlobalHost.DependencyResolver = new AutofacDependencyResolver(container);

            app.MapSignalR();
        }
    }
}
