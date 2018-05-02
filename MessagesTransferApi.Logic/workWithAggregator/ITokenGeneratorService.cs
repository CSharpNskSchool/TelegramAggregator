using System;
using System.Collections.Generic;
using System.Text;

namespace MessagesTransferApi.Logic
{
    public interface ITokenGeneratorService
    {
        string GenerateToken(string Login);
    }
}
