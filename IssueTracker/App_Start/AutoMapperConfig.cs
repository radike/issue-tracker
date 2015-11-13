using AutoMapper;
using IssueTracker.Entities;
using IssueTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            //ApplicationUserMapping();
            CommentMapping();
            IssueMapping();
            ProjectMapping();
            StateMapping();
            StateWorkflowMapping();
        }
        
        private static void CommentMapping()
        {
            Mapper.CreateMap<Comment, CommentViewModel>();
            Mapper.CreateMap<CommentViewModel, Comment>();
        }

        private static void IssueMapping()
        {
            Mapper.CreateMap<Issue, IssueIndexViewModel>();
            Mapper.CreateMap<IssueIndexViewModel, Issue>();
            Mapper.CreateMap<IssueCreateViewModel, Issue>();
            Mapper.CreateMap<IssueEditViewModel, Issue>();
            Mapper.CreateMap<Issue, IssueEditViewModel>();
        }

        private static void ProjectMapping()
        {
            Mapper.CreateMap<Project, ProjectViewModel>();
            Mapper.CreateMap<ProjectViewModel, Project>();
        }

        private static void StateMapping()
        {
            Mapper.CreateMap<State, StateViewModel>();
            Mapper.CreateMap<StateViewModel, State>();
        }

        private static void StateWorkflowMapping()
        {
            Mapper.CreateMap<StateWorkflow, StateWorkflowViewModel>();
            Mapper.CreateMap<StateWorkflowViewModel, StateWorkflow>();
        }
    }
}