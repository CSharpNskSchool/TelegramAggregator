namespace TelegramAggregator.Data.Entities
{
    public class BotUser
    {
        public int Id { get; set; }
        public int TelegramId { get; set; }
        public VkAccount VkAccount { get; set; }
    }
}