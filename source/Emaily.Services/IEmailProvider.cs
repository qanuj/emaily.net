namespace Emaily.Services
{
    public interface IEmailProvider
    {
        void VerifyEmail(string email);
    }
}