using System;
using System.Collections.Generic;
using System.Text;

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
