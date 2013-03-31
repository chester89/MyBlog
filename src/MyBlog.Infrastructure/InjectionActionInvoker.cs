using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using StructureMap;

namespace MyBlog.Infrastructure
{
    public class InjectingActionInvoker : ControllerActionInvoker
    {
        private readonly IContainer container;

        public InjectingActionInvoker(IContainer container)
        {
            this.container = container;
        }

        protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var info = base.GetFilters(controllerContext, actionDescriptor);

            foreach (var authorizationFilter in info.AuthorizationFilters)
            {
                container.BuildUp(authorizationFilter);
            }
            foreach (var actionFilter in info.ActionFilters)
            {
                container.BuildUp(actionFilter);
            }
            foreach (var resultFilter in info.ResultFilters)
            {
                container.BuildUp(resultFilter);
            }
            foreach (var exceptionFilter in info.ExceptionFilters)
            {
                container.BuildUp(exceptionFilter);
            }

            return info;
        }
    }
}
