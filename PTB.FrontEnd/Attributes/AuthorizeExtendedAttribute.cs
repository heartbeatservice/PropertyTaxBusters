using PTB.Entities;
using PTB.Entities.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;

namespace PTB.FrontEnd.Attributes
{
    public class AuthorizeExtendedAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext actionContext)
        {
            base.OnAuthorization(actionContext);
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            var user = StateController.GetUserSession();

            if (user == null)
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }


            IsUserOkToProceed(actionContext, user);



            //HandleUnauthorizedRequest(actionContext);
        }

        private void IsUserOkToProceed(AuthorizationContext actionContext, UserDetailModel user)
        {
         
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RouteValueDictionary routeValueDict = null;

            routeValueDict = new RouteValueDictionary
                {
                    {"action", "Login"},
                    {"controller", "Account"},
                    {"area", ""}
                };


            // Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
            filterContext.Result = new RedirectToRouteResult(routeValueDict);
        }
    }
}