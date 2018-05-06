using System.ComponentModel.DataAnnotations;

namespace CommunicationModels.Models
{
    public class UserData
    {
        [Required(ErrorMessage = "Login должен быть определен")]
        public string Login { get; set; }

        [Required(ErrorMessage = "URL должен быть определен")]
        public string Url { get; set; }
    }
}