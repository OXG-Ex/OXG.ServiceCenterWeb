using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OXG.ServiceCenterWeb.Models
{
    public class Equipment
    {
        public int Id { get; set; }

        [Display(Name = "Оборудование")]
        [Required(ErrorMessage = "Введите наименование", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Display(Name = "Аксессуары (З/у, сумки и т.д.)")]
        public string Accesories { get; set; }
    }
}
