using System.ComponentModel.DataAnnotations;
using CommunicationModels.Models;

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