//Progam.cs => builder.Services.AddHostedService<ConnectionMonitorService>();
namespace ActiveDirectoryWebApi.Services;

public class ConnectionMonitorService : IHostedService, IDisposable
{
    private readonly ILogger<ConnectionMonitorService> _logger;
    private readonly LdapConnection _ldapConnection;
    private readonly LdapConfigs _ldapConfigs;
    private readonly IConfiguration _configuration;
    private Timer _timer;

    public ConnectionMonitorService(LdapConnection ldapConnection, IConfiguration configuration, ILogger<ConnectionMonitorService> logger)
    {
        _ldapConnection = ldapConnection;
        _configuration = configuration;
        _ldapConfigs = _configuration.GetSection(nameof(LdapConfigs)).Get<LdapConfigs>();
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(CheckConnection, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    private void CheckConnection(object state)
    {
        try
        {
            _ldapConnection.Bind();
        }
        catch (Exception ex)
        {
            _logger.LogError("LDAP connection failed: {ex.Message}. Attempting to restart.", ex.Message);
            RestartConnection();
        }
    }

    private void RestartConnection()
    {
        try
        {
            var endpoint = new LdapDirectoryIdentifier(_ldapConfigs.Server, false, false);
            var ldapConnection = new LdapConnection(endpoint,
                new NetworkCredential($"{_ldapConfigs.User}@{_ldapConfigs.DomainName}", _ldapConfigs.Pass))
            {
                AuthType = AuthType.Basic
            };
            ldapConnection.SessionOptions.ReferralChasing = ReferralChasingOptions.None;
            ldapConnection.Bind();
            _logger.LogInformation("LDAP connection restarted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to restart LDAP connection: {ex.Message}", ex.StackTrace);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
