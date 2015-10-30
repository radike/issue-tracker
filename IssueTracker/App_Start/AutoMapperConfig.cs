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
            Mapper.CreateMap<Comment, CommentViewModel>()
                .ForMember(view => view.Id, options => options.MapFrom(c => c.Id))
                .ForMember(view => view.AuthorId, options => options.MapFrom(c => c.AuthorId))
                .ForMember(view => view.User, options => options.MapFrom(c => c.User))
                .ForMember(view => view.Issue, options => options.MapFrom(c => c.Issue))
                .ForMember(view => view.IssueId, options => options.MapFrom(c => c.IssueId))
                .ForMember(view => view.Posted, options => options.MapFrom(c => c.Posted))
                .ForMember(view => view.Text, options => options.MapFrom(c => c.Text));
            Mapper.CreateMap<CommentViewModel, Comment>()
                .ForMember(view => view.Id, options => options.MapFrom(c => c.Id))
                .ForMember(view => view.AuthorId, options => options.MapFrom(c => c.AuthorId))
                .ForMember(view => view.User, options => options.MapFrom(c => c.User))
                .ForMember(view => view.Issue, options => options.MapFrom(c => c.Issue))
                .ForMember(view => view.IssueId, options => options.MapFrom(c => c.IssueId))
                .ForMember(view => view.Posted, options => options.MapFrom(c => c.Posted))
                .ForMember(view => view.Text, options => options.MapFrom(c => c.Text));
        }

        private static void IssueMapping()
        {
            Mapper.CreateMap<Issue, IssueViewModel>()
                .ForMember(view => view.Id, options => options.MapFrom(i => i.Id))
                .ForMember(view => view.Project, options => options.MapFrom(i => i.Project))
                .ForMember(view => view.ProjectId, options => options.MapFrom(i => i.ProjectId))
                .ForMember(view => view.State, options => options.MapFrom(i => i.State))
                .ForMember(view => view.StateId, options => options.MapFrom(i => i.StateId))
                .ForMember(view => view.Name, options => options.MapFrom(i => i.Name))
                .ForMember(view => view.Assignee, options => options.MapFrom(i => i.Assignee))
                .ForMember(view => view.AssigneeId, options => options.MapFrom(i => i.AssigneeId))
                .ForMember(view => view.Reporter, options => options.MapFrom(i => i.Reporter))
                .ForMember(view => view.ReporterId, options => options.MapFrom(i => i.ReporterId))
                .ForMember(view => view.Created, options => options.MapFrom(i => i.Created))
                .ForMember(view => view.Description, options => options.MapFrom(i => i.Description))
                .ForMember(view => view.Comments, options => options.MapFrom(i => i.Comments));
            Mapper.CreateMap<IssueViewModel, Issue>()
                .ForMember(view => view.Id, options => options.MapFrom(i => i.Id))
                .ForMember(view => view.Project, options => options.MapFrom(i => i.Project))
                .ForMember(view => view.ProjectId, options => options.MapFrom(i => i.ProjectId))
                .ForMember(view => view.State, options => options.MapFrom(i => i.State))
                .ForMember(view => view.StateId, options => options.MapFrom(i => i.StateId))
                .ForMember(view => view.Name, options => options.MapFrom(i => i.Name))
                .ForMember(view => view.Assignee, options => options.MapFrom(i => i.Assignee))
                .ForMember(view => view.AssigneeId, options => options.MapFrom(i => i.AssigneeId))
                .ForMember(view => view.Reporter, options => options.MapFrom(i => i.Reporter))
                .ForMember(view => view.ReporterId, options => options.MapFrom(i => i.ReporterId))
                .ForMember(view => view.Created, options => options.MapFrom(i => i.Created))
                .ForMember(view => view.Description, options => options.MapFrom(i => i.Description))
                .ForMember(view => view.Comments, options => options.MapFrom(i => i.Comments));
            Mapper.CreateMap<IssueCreateViewModel, Issue>()
                .ForMember(view => view.ProjectId, options => options.MapFrom(i => i.ProjectId))
                .ForMember(view => view.AssigneeId, options => options.MapFrom(i => i.AssigneeId))
                .ForMember(view => view.Name, options => options.MapFrom(i => i.Name))
                .ForMember(view => view.Description, options => options.MapFrom(i => i.Description));
            Mapper.CreateMap<IssueEditViewModel, Issue>()
                .ForMember(view => view.Id, options => options.MapFrom(i => i.Id))
                .ForMember(view => view.ProjectId, options => options.MapFrom(i => i.ProjectId))
                .ForMember(view => view.AssigneeId, options => options.MapFrom(i => i.AssigneeId))
                .ForMember(view => view.Name, options => options.MapFrom(i => i.Name))
                .ForMember(view => view.Description, options => options.MapFrom(i => i.Description));
            Mapper.CreateMap<Issue, IssueEditViewModel>()
                .ForMember(view => view.Id, options => options.MapFrom(i => i.Id))
                .ForMember(view => view.ProjectId, options => options.MapFrom(i => i.ProjectId))
                .ForMember(view => view.AssigneeId, options => options.MapFrom(i => i.AssigneeId))
                .ForMember(view => view.Name, options => options.MapFrom(i => i.Name))
                .ForMember(view => view.Description, options => options.MapFrom(i => i.Description));
        }

        private static void ProjectMapping()
        {
            Mapper.CreateMap<Project, ProjectViewModel>()
               .ForMember(view => view.Id, options => options.MapFrom(p => p.Id))
               .ForMember(view => view.Title, options => options.MapFrom(p => p.Title))
               .ForMember(view => view.Issues, options => options.MapFrom(p => p.Issues))
               .ForMember(view => view.SelectedUsers, options => options.MapFrom(p => p.SelectedUsers))
               .ForMember(view => view.Users, options => options.MapFrom(p => p.Users));
            Mapper.CreateMap<ProjectViewModel, Project>()
               .ForMember(view => view.Id, options => options.MapFrom(p => p.Id))
               .ForMember(view => view.Title, options => options.MapFrom(p => p.Title))
               .ForMember(view => view.Issues, options => options.MapFrom(p => p.Issues))
               .ForMember(view => view.SelectedUsers, options => options.MapFrom(p => p.SelectedUsers))
               .ForMember(view => view.Users, options => options.MapFrom(p => p.Users));
        }

        private static void StateMapping()
        {
            Mapper.CreateMap<State, StateViewModel>()
              .ForMember(view => view.Id, options => options.MapFrom(s => s.Id))
              .ForMember(view => view.Title, options => options.MapFrom(s => s.Title))
              .ForMember(view => view.IsInitial, options => options.MapFrom(s => s.IsInitial))
              .ForMember(view => view.Colour, options => options.MapFrom(s => s.Colour));
            Mapper.CreateMap<StateViewModel, State>()
              .ForMember(view => view.Id, options => options.MapFrom(s => s.Id))
              .ForMember(view => view.Title, options => options.MapFrom(s => s.Title))
              .ForMember(view => view.IsInitial, options => options.MapFrom(s => s.IsInitial))
              .ForMember(view => view.Colour, options => options.MapFrom(s => s.Colour));
        }

        private static void StateWorkflowMapping()
        {
            Mapper.CreateMap<StateWorkflow, StateWorkflowViewModel>()
              .ForMember(view => view.Id, options => options.MapFrom(s => s.Id))
              .ForMember(view => view.FromState, options => options.MapFrom(s => s.FromState))
              .ForMember(view => view.FromStateId, options => options.MapFrom(s => s.FromStateId))
              .ForMember(view => view.ToState, options => options.MapFrom(s => s.ToState))
              .ForMember(view => view.ToStateId, options => options.MapFrom(s => s.ToStateId));
            Mapper.CreateMap<StateWorkflowViewModel, StateWorkflow>()
              .ForMember(view => view.Id, options => options.MapFrom(s => s.Id))
              .ForMember(view => view.FromState, options => options.MapFrom(s => s.FromState))
              .ForMember(view => view.FromStateId, options => options.MapFrom(s => s.FromStateId))
              .ForMember(view => view.ToState, options => options.MapFrom(s => s.ToState))
              .ForMember(view => view.ToStateId, options => options.MapFrom(s => s.ToStateId));
        }
    }
}