namespace TelegramAggregator.Data.Entities
{
    public class BotUser
    {
        public int Id { get; set; }
        public int TelegramUserId { get; set; }
        public long TelegramChatId { get; set; }
        public VkAccount VkAccount { get; set; }
    }
}