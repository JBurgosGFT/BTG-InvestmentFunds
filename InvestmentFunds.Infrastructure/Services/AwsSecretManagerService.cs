using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

public class AwsSecretManagerService
{
    private readonly IAmazonSecretsManager _client;
    public AwsSecretManagerService(IAmazonSecretsManager client) => _client = client;

    public async Task<string?> GetSecretAsync(string secretName)
    {
        var response = await _client.GetSecretValueAsync(new GetSecretValueRequest { SecretId = secretName });
        return response.SecretString;
    }
}