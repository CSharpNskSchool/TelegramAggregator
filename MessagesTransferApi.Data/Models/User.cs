using System.Collections.Generic;

namespace MessagesTransferApi.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string UserToken { get; set; }

        public string FeedbackUrl { get; set; }
    }
}