using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SPAWedding.Core.Models;
using SPAWedding.Web.Models;
using Microsoft.AspNet.Identity;

namespace SPAWedding.Web.Providers
{
    public class CustomerAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Customer/Auth/Login/?returnUrl=/Shop/Checkout");

        }
    }
}