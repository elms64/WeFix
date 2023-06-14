using System;
using System.Threading.Tasks;
using WeFix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace WeFix.Authorization
{
    public class TechnicianHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, Appointment>
    {
        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   Appointment resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            // If not asking for approval/reject, return.
            if (requirement.Name != Constants.ApproveOperationName &&
                requirement.Name != Constants.RejectOperationName)
            {
                return Task.CompletedTask;
            }

            // Technicians can approve or reject.
            if (context.User.IsInRole(Constants.ReceptionRole))
            {
                context.Succeed(requirement);
            }


            return Task.CompletedTask;
        }
    }
}