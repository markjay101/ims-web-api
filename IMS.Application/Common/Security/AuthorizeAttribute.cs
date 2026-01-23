namespace IMS.Application.Common.Security
{
    public class AuthorizeAttribute : Attribute
    {
        public string Role { get; set; } = string.Empty;
    }
}
