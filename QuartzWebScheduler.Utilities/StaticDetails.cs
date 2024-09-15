using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuartzWebScheduler.Utility
{
    public class StaticDetails
    {
        //Roles
        public const string RoleAdmin = "Admin";
        public const string RoleUser = "User";
        //LogTypes
        public const string LogTypeDebug = "Debug";
        public const string LogTypeInformation = "Information";
        public const string LogTypeWarning = "Warning";
        public const string LogTypeError = "Error";
        public const string LogTypeFatal = "Fatal";
        //RequestTypes
        public List<SelectListItem> RequestTypes =
        [
            new SelectListItem { Value = "GET", Text = "GET" },
            new SelectListItem { Value = "POST", Text = "POST" },
            new SelectListItem { Value = "PUT", Text = "PUT" },
            new SelectListItem { Value = "DELETE", Text = "DELETE" },
            new SelectListItem { Value = "PATCH", Text = "PATCH" }
        ];
    }
}
