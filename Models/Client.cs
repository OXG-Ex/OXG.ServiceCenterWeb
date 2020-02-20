using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OXG.ServiceCenterWeb.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Display(Name = "Имя клиента")]
        [Required(ErrorMessage ="Введите имя клиента",AllowEmptyStrings =false)]
        public string Name { get; set; }

        [Display(Name = "Номер телефона")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Введите номер телефона", AllowEmptyStrings = false)]
        public string Phone { get; set; }

        [Display(Name = "Номер скидочной карты")]
        public int DiscountNumber { get; set; }
    }
}
