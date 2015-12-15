using Autofac;
using Common.Data.Core.Contracts;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Data.Data_Repositories;
using IssueTracker.Data.Facade;
using IssueTracker.Data.Services;
using IssueTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Data
{
    public class DataModule : Module
    {
        private string connStr;
        public DataModule(string connString)
        {
            this.connStr = connString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new IssueTrackerContext()).As<IDbContext>().InstancePerRequest();
            builder.RegisterType<StateRepository>().As<IStateRepository>().InstancePerRequest();
            builder.RegisterType<StateWorkflowRepository>().As<IStateWorkflowRepository>().InstancePerRequest();
            builder.RegisterType<ProjectRepository>().As<IProjectRepository>().InstancePerRequest();
            builder.RegisterType<IssueRepository>().As<IIssueRepository>().InstancePerRequest();
            builder.RegisterType<CommentRepository>().As<ICommentRepository>().InstancePerRequest();
            builder.RegisterType<ApplicationUserRepository>().As<IApplicationUserRepository>().InstancePerRequest();
            builder.RegisterType<StateService>().As<IStateService>().InstancePerRequest();
            builder.RegisterType<ProjectService>().As<IProjectService>().InstancePerRequest();
            builder.RegisterType<IssueService>().As<IIssueService>().InstancePerRequest();
            builder.RegisterType<IssueTrackerFacade>().As<IFacade>().InstancePerRequest();
            base.Load(builder);
        }
    }
}
