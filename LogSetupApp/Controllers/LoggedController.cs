using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LogSetupApp.Controllers
{
    public abstract class LoggedController : Controller
    {

        private readonly ILogger _logger;

        protected LoggedController(
            ILogger logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                string logMessage = null;
                var path = context.HttpContext.Request.Path.ToString().ToLower();
                if (path.Contains("auth") ||
                    path.Contains("login") ||
                    path.Contains("password") ||
                    path.Contains("pw"))
                {
                    logMessage = "";
                }
                else
                {
                    logMessage = JsonConvert.SerializeObject(new
                    { actionArguments = context.ActionArguments, routeDataValues = context.RouteData?.Values }, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                }
                _logger.LogInformation(logMessage);

            }
            catch (Exception ex)
            {
                _logger.LogError(context.HttpContext.Request.Path, ex);
            }


            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                string logMessage = null;
                var path = context.HttpContext.Request.Path.ToString().ToLower();
                if (path.Contains("auth") ||
                    path.Contains("login") ||
                    path.Contains("password") ||
                    path.Contains("pw"))
                {
                    logMessage = "";
                }
                else
                {
                    logMessage = JsonConvert.SerializeObject(new { result = context.Result, routeDataValues = context.RouteData?.Values }, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                }
                _logger.LogInformation(logMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(context.HttpContext.Request.Path, ex);
            }
            base.OnActionExecuted(context);
        }


    }

}
