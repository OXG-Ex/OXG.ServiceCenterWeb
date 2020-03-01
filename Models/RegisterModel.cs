using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OXG.ServiceCenterWeb.Models
{
    public class RegisterModel
    {
        [Display(Name ="Электронная почта")]
        [Required(ErrorMessage ="Введите email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"(?i)[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[A-Z]{2,4}\b", ErrorMessage ="Неверный Email")]
        public string Email { get; set; }

        [Display(Name = "Введите пароль")]
        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Подтвердите пароль")]
        [Required(ErrorMessage = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Пароли не совпадают")]
        public string ConfirmPasswod { get; set; }
    }
}
