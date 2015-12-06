using Autofac;
using Common.Data.Core.Contracts;
using IssueTracker.Data.Contracts.Repository_Interfaces;
using IssueTracker.Data.Data_Repositories;
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

            base.Load(builder);
        }
    }
}
