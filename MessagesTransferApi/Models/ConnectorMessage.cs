using CommunicationModels.Models;
using System.ComponentModel.DataAnnotations;

namespace MessagesTransferApi.Models
{
    public class ConnectorMessage
    {
        [Required(ErrorMessage = "You need to input receiver token")]
        public string UserToken { get; set; }

        [Required(ErrorMessage = "You need to input message")]
        public RecievedMessage Message { get; set; }
    }
}
