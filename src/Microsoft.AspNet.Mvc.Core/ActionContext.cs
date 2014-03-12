﻿using System.Collections.Generic;
using Microsoft.AspNet.Abstractions;
using Microsoft.AspNet.Routing;

namespace Microsoft.AspNet.Mvc
{
    public class ActionContext
    {
        public ActionContext(HttpContext httpContext, IRouter router, IDictionary<string, object> routeValues, ActionDescriptor actionDescriptor)
        {
            HttpContext = httpContext;
            Router = router;
            RouteValues = routeValues;
            ActionDescriptor = actionDescriptor;
        }

        public HttpContext HttpContext { get; private set; }

        public IRouter Router { get; private set; }

        public IDictionary<string, object> RouteValues { get; private set; }

        public ActionDescriptor ActionDescriptor { get; private set; }
    }
}