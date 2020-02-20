using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OXG.ServiceCenterWeb.Models
{
    public class Component
    {
        public int Id { get; set; }

        [Display(Name = "Название компонента")]
        [Required(ErrorMessage = "Введите название компонента", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Display(Name = "Тип")]
        [Required(ErrorMessage = "Выберите тип компонента", AllowEmptyStrings = false)]
        public string OfType { get; set; }

        [Display(Name = "Состояние")]
        public bool NewComponent { get; set; }

        [Display(Name = "Стоимость")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
