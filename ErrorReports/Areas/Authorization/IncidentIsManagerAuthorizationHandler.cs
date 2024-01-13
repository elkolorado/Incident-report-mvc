using System.Threading.Tasks;
using ErrorReports.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace ErrorReports.Authorization
{
    public class IncidentIsManagerAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, ErrorReport>
    {
        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   ErrorReport resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            // If not asking for approval/reject, return.
            if (requirement.Name != Constants.AssignOperationName && 
                requirement.Name != Constants.AcceptOperationName &&
                requirement.Name != Constants.CommentOperationName &&
                requirement.Name != Constants.UpdateOperationName && 
                requirement.Name != Constants.DeleteOperationName &&
                requirement.Name != Constants.ReadOperationName)
            {
                return Task.CompletedTask;
            }

            // Managers can approve or reject.
            if (context.User.IsInRole(Constants.IncidentManagersRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}