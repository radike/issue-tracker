using AutoMapper;
using IssueTracker.ViewModels;
using IssueTracker.Entities;

namespace IssueTracker
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            commentMapping();
            issueMapping();
            projectMapping();
            stateMapping();
            stateWorkflowMapping();
            userMappging();
        }
        
        private static void commentMapping()
        {
            Mapper.CreateMap<Comment, CommentViewModel>();
            Mapper.CreateMap<CommentViewModel, Comment>();
        }

        private static void issueMapping()
        {
            Mapper.CreateMap<Issue, IssueIndexViewModel>();
            Mapper.CreateMap<IssueIndexViewModel, Issue>();
            Mapper.CreateMap<IssueCreateViewModel, Issue>();
            Mapper.CreateMap<IssueEditViewModel, Issue>();
            Mapper.CreateMap<Issue, IssueEditViewModel>();
        }

        private static void projectMapping()
        {
            Mapper.CreateMap<Project, ProjectViewModel>();
            Mapper.CreateMap<ProjectViewModel, Project>();
        }

        private static void stateMapping()
        {
            Mapper.CreateMap<State, StateViewModel>();
            Mapper.CreateMap<StateViewModel, State>();
        }

        private static void stateWorkflowMapping()
        {
            Mapper.CreateMap<StateWorkflow, StateWorkflowViewModel>();
            Mapper.CreateMap<StateWorkflowViewModel, StateWorkflow>();
        }

        private static void userMappging()
        {
            Mapper.CreateMap<ApplicationUser, UserEditViewModel>();
            Mapper.CreateMap<UserEditViewModel, ApplicationUser>();
        }
    }
}