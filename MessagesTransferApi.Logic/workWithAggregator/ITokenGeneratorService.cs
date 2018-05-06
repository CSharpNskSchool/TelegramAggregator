namespace MessagesTransferApi.Logic
{
    public interface ITokenGeneratorService
    {
        string GenerateToken(string Login);
    }
}