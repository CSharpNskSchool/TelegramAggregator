namespace MessagesTransferApi.Data.Models
{
    public class NetworkAuthData
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public string PlatformName { get; set; }

        public string AccessToken { get; set; }
    }
}