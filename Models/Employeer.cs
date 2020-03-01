using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OXG.ServiceCenterWeb.Models
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    public class Employeer
    {
        
        public int Id { get; set; }

        [Display(Name = "Имя сотрудника")]
        public string Name { get; set; }

        [Display(Name = "Фото сотрудника")]
        [DataType(DataType.Upload)]
        public string Photo { get; set; }

        [Display(Name = "ИНН Сотрудника")]
        public string INN { get; set; } = "";

        [Display(Name = "Специализация")]
        public string Specialization { get; set; } = "";

        [Display(Name = "Процент")]
        public double Percent { get; set; }

        [Display(Name = "Рейтинг (0-100)")]
        public byte Reit { get; set; }

        [Display(Name = "Баланс")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        [Display(Name = "Электронная почта")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"(?i)[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[A-Z]{2,4}\b")]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        //TODO: добавить метаданные сюда
        public int? RoleId { get; set; }
        public Role Role { get; set; }
    }
}
