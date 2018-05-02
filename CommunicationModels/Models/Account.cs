using System.ComponentModel.DataAnnotations;

namespace CommunicationModels.Models
{
    public class Account
    {
        [Required(ErrorMessage = "NetworkName должно быть указано")]
        public string NetworkName { get; set; }

        [Required(ErrorMessage = "AccessToken должен быть указан")]
        public string AccessToken { get; set; }
    }
}
