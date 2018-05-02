using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MessagesTransferApi.Models
{
    public class ConnectorData
    {
        [Required(ErrorMessage = "You need to input network name")]
        public string NetworkName { get; set; }

        [Required(ErrorMessage = "You need to input url")]
        public string Url { get; set; }
    }
}
