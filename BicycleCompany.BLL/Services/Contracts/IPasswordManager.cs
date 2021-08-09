namespace BicycleCompany.BLL.Services.Contracts
{
    public interface IPasswordManager
    {
        string GetPasswordHash(string password, string salt);
        string GenerateSalt();
    }
}
