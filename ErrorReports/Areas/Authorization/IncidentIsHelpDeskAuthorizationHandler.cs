using System.Threading.Tasks;
using ErrorReports.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace ErrorReports.Authorization
{
    public class IncidentIsHelpDeskAuthorizationHandler :
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
            if (requirement.Name != Constants.AcceptOperationName &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.CommentOperationName &&
                requirement.Name != Constants.AssignOperationName &&
                requirement.Name != Constants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }

            // Helpdesk can read/comment/accept tasks.
            if (context.User.IsInRole(Constants.IncidentHelpDeskRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}