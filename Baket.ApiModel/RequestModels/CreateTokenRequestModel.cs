using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.ApiModel.RequestModels
{
    public class CreateTokenRequestModel
    {
        [Required(AllowEmptyStrings = false)
            ]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
