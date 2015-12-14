﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IssueTracker.Locale {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ProjectStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ProjectStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("IssueTracker.Locale.Project.ProjectStrings", typeof(ProjectStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Project.
        /// </summary>
        public static string EntityName {
            get {
                return ResourceManager.GetString("EntityName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only project administrators can delete projects..
        /// </summary>
        public static string ErrorMessageDeleteNonadmin {
            get {
                return ResourceManager.GetString("ErrorMessageDeleteNonadmin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only project owners and administrators can edit projects..
        /// </summary>
        public static string ErrorMessageEditNonadmin {
            get {
                return ResourceManager.GetString("ErrorMessageEditNonadmin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Entered code has invalid format. Only characters are allowed..
        /// </summary>
        public static string ErrorMessageInvalidCode {
            get {
                return ResourceManager.GetString("ErrorMessageInvalidCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Entered code is already associated with another project..
        /// </summary>
        public static string ErrorMessageNotUniqueCode {
            get {
                return ResourceManager.GetString("ErrorMessageNotUniqueCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The project has no issues..
        /// </summary>
        public static string HasNoIssues {
            get {
                return ResourceManager.GetString("HasNoIssues", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No projects found. Create a {0}..
        /// </summary>
        public static string ListNoResult {
            get {
                return ResourceManager.GetString("ListNoResult", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to new project.
        /// </summary>
        public static string ListNoResultNew {
            get {
                return ResourceManager.GetString("ListNoResultNew", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Code.
        /// </summary>
        public static string ProjectCode {
            get {
                return ResourceManager.GetString("ProjectCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Project owner.
        /// </summary>
        public static string ProjectOwner {
            get {
                return ResourceManager.GetString("ProjectOwner", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Title.
        /// </summary>
        public static string ProjectTitle {
            get {
                return ResourceManager.GetString("ProjectTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Project Users.
        /// </summary>
        public static string ProjectUsers {
            get {
                return ResourceManager.GetString("ProjectUsers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to My projects.
        /// </summary>
        public static string TabMyProjects {
            get {
                return ResourceManager.GetString("TabMyProjects", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Project details.
        /// </summary>
        public static string TitleDetails {
            get {
                return ResourceManager.GetString("TitleDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Project &apos;{0}&apos; details.
        /// </summary>
        public static string TitleDetailsIncludeName {
            get {
                return ResourceManager.GetString("TitleDetailsIncludeName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Browse projects.
        /// </summary>
        public static string TitleIndex {
            get {
                return ResourceManager.GetString("TitleIndex", resourceCulture);
            }
        }
    }
}