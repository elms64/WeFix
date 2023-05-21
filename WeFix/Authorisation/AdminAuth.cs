using System.Threading.Tasks;
using WeFix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.VisualBasic;

namespace WeFix.Authorization
{
    /*  public class AdminAuth
                      : AuthorizationHandler<OperationAuthorizationRequirement, Part>
      {
          protected override Task HandleRequirementAsync(
                                                AuthorizationHandlerContext context,
                                      OperationAuthorizationRequirement requirement,
                                       Part resource)
          {
              if (context.User == null)
              {
                  return Task.CompletedTask;
              }

              // Administrators can do anything.
              if (context.User.IsInRole(Constants.PartAdministratorsRole))
              {
                  context.Succeed(requirement);
              }

              return Task.CompletedTask;
          }
      } */
}
