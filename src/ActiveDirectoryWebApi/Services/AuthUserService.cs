namespace ActiveDirectoryWebApi.Services;

public class AuthUserService
{
    private readonly IConfiguration _configuration;

    public AuthUserService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool AuthUser(string username, string password)
    {
        var configs = _configuration.GetSection(nameof(LdapConfigs)).Get<LdapConfigs>();
        try
        {
            var endpoint = new LdapDirectoryIdentifier(configs.Server, configs.Port, configs.UseSSL ? true : false, false);
            var ldapConnection = new LdapConnection(endpoint,
            new NetworkCredential($"{username}@{configs.DomainName}", password))
            {
                AuthType = AuthType.Basic
            };
            if (configs.UseSSL && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ldapConnection.SessionOptions.SecureSocketLayer = true;
                ldapConnection.SessionOptions.VerifyServerCertificate = (con, cert) => true;
            }
            if (configs.UseSSL && RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                ldapConnection.SessionOptions.SecureSocketLayer = true;
            }
            ldapConnection.SessionOptions.ReferralChasing = ReferralChasingOptions.None;
            ldapConnection.Bind();
            return true;
        }
        catch (LdapException)
        {
            return false;
        }
    }
}
