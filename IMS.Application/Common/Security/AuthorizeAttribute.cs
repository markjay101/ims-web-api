namespace IMS.Application.Common.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeAttribute : Attribute
    {
        public string Role { get; set; } = string.Empty;
    }
}
