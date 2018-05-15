using System.ComponentModel.DataAnnotations;

namespace CommunicationModels.Models
{
    public class ConnectorData
    {
        [Required(ErrorMessage = "You need to input network name")]
        public string NetworkName { get; set; }

        [Required(ErrorMessage = "You need to input url")]
        public string Url { get; set; }
    }
}