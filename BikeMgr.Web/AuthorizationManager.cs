using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace BikeMgrWeb
{
    public class AuthorizationManager : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            switch (context.Resource.First().Value)
            {
                case "Generic":
                    return AuthorizeGeneric(context);
                default:
                    return Nok();
            }
        }

        private Task<bool> AuthorizeGeneric(ResourceAuthorizationContext context)
        {
            switch (context.Action.First().Value)
            {
                case "Read":
                    return Eval(context.Principal.HasClaim("role", "Read"));
                case "Write":
                    return Eval(context.Principal.HasClaim("role", "Write"));
                case "Admin":
                    return Eval(context.Principal.HasClaim("role", "Admin"));
                case "UserAdmin":
                    return Eval(context.Principal.HasClaim("role", "UserAdmin"));
                default:
                    return Nok();
            }
        }
    }
}