namespace MaterialAdvisor.Application.Services.Abstraction;

public interface ISecurityService
{
    string GetHash(string input);

    string Encrypt(string input);

    string Decrypt(string input);
}