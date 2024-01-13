using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ErrorReports.Authorization
{
    public static class IncidentOperations
    {
        public static OperationAuthorizationRequirement Create =
          new OperationAuthorizationRequirement { Name = Constants.CreateOperationName };
        public static OperationAuthorizationRequirement Read =
          new OperationAuthorizationRequirement { Name = Constants.ReadOperationName };
        public static OperationAuthorizationRequirement Update =
          new OperationAuthorizationRequirement { Name = Constants.UpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
          new OperationAuthorizationRequirement { Name = Constants.DeleteOperationName };
        public static OperationAuthorizationRequirement Accept =
          new OperationAuthorizationRequirement { Name = Constants.AcceptOperationName };
        public static OperationAuthorizationRequirement Assign =
          new OperationAuthorizationRequirement { Name = Constants.AssignOperationName };
        public static OperationAuthorizationRequirement Comment =
  new OperationAuthorizationRequirement { Name = Constants.CommentOperationName };
    }

    public class Constants
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperationName = "Read";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";
        public static readonly string AssignOperationName = "Assign";
        public static readonly string AcceptOperationName = "Accept";
        public static readonly string CommentOperationName = "Comment";



        public static readonly string IncidentAdministratorsRole =
                                                              "ContactAdministrators";
        public static readonly string IncidentManagersRole = "ContactManagers";
        public static readonly string IncidentHelpDeskRole = "ContactHelpDesk";
    }
}