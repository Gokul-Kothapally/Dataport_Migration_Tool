using ArchivingTool.Enums;

namespace ArchivingTool.Model.Arms
{
    public class CommonRequestModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ApiToken { get; set; }
        public string Endpoint { get; set; } = null!;
        public HttpContent PostContent { get; set; } = null!;
        public HttpMethod RequestMethod { get; set; } = null!;
        public AuthorizationMethod RequestAuthMethod { get; set; }
    }
}
