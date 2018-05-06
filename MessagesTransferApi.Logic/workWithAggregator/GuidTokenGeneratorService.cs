using System;

namespace MessagesTransferApi.Logic
{
    public class GuidTokenGeneratorService : ITokenGeneratorService
    {
        public string GenerateToken(string Login)
        {
            return Guid.NewGuid().ToString();
        }
    }
}