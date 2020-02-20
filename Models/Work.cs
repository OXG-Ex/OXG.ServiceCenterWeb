using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OXG.ServiceCenterWeb.Models
{
    public class Work
    {
        public int Id { get; set; }

        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [Display(Name = "Кол-во")]
        public byte Num { get; set; }

        [Display(Name = "Стоимость")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
