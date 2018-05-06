using System.ComponentModel.DataAnnotations;

namespace CommunicationModels.Models
{
    public class AggregatorMessage
    {
        [Required(ErrorMessage = "NetworkName должен быть определен")]
        public string NetworkName { get; set; }

        [Required(ErrorMessage = "Сообщение должно быть определено")]
        public Message Message { get; set; }
    }
}