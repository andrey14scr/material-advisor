namespace MaterialAdvisor.Application.Services;

public interface ISecurityService
{
    string GetHash(string input);

    string Encrypt(string input);

    string Decrypt(string input);
}