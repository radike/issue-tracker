using System;
using System.Collections.Generic;
using IssueTracker.Models;

namespace IssueTracker.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DAL.ApplicationDbContext context)
        {
            /*
            var open = new State { Id = Guid.NewGuid(), Title = "Open", IsInitial = true };
            var toSolve = new State { Id = Guid.NewGuid(), Title = "To Solve", IsInitial = false };
            var toCancel = new State { Id = Guid.NewGuid(), Title = "To Cancel", IsInitial = false };
            var inProgress = new State { Id = Guid.NewGuid(), Title = "In Progress", IsInitial = false };
            var progressSuspended = new State { Id = Guid.NewGuid(), Title = "Progress Suspended", IsInitial = false };
            var readyForDeployment = new State { Id = Guid.NewGuid(), Title = "Ready for deployment", IsInitial = false };
            var toTest = new State { Id = Guid.NewGuid(), Title = "To Test", IsInitial = false };
            var inTesting = new State { Id = Guid.NewGuid(), Title = "In Testing", IsInitial = false };
            var closed = new State { Id = Guid.NewGuid(), Title = "Closed", IsInitial = false };
            var cancelled = new State { Id = Guid.NewGuid(), Title = "Cancelled", IsInitial = false };
            var testSuspended = new State { Id = Guid.NewGuid(), Title = "Test Suspended", IsInitial = false };

            var states = new List<State>
            {
                open,
                toSolve,
                toCancel,
                inProgress,
                progressSuspended,
                readyForDeployment,
                toTest,
                inTesting,
                closed,
                cancelled,
                testSuspended
            };

            var stateWorkflows = new List<StateWorkflow>
            {
                new StateWorkflow {Id = Guid.NewGuid(), FromState = open, ToState = toSolve},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = toSolve, ToState = toCancel},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = toSolve, ToState = inProgress},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = inProgress, ToState = progressSuspended},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = inProgress, ToState = readyForDeployment},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = inProgress, ToState = toTest},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = readyForDeployment, ToState = toTest},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = toTest, ToState = inTesting},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = toTest, ToState = closed},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = toTest, ToState = toSolve},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = toCancel, ToState = toSolve},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = toCancel, ToState = cancelled},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = inTesting, ToState = testSuspended},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = inTesting, ToState = toSolve},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = inTesting, ToState = closed},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = progressSuspended, ToState = inProgress},
                new StateWorkflow {Id = Guid.NewGuid(), FromState = testSuspended, ToState = inTesting}
            };

            context.Database.ExecuteSqlCommand("delete from [dbo].[Issue]");
            context.Database.ExecuteSqlCommand("delete from [dbo].[StateWorkflow]");
            context.Database.ExecuteSqlCommand("delete from [dbo].[State]"); 

            states.ForEach(p => context.States.Add(p));
            stateWorkflows.ForEach(p => context.StateWorkflows.Add(p));

            context.SaveChanges();
            */
        }
    }
}
