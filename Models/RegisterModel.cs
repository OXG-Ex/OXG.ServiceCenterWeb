using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OXG.ServiceCenterWeb.Models
{
    /// <summary>
    /// Модель использующаяся для регистрации нового аккаунта
    /// </summary>
    public class RegisterModel
    {
        [Display(Name ="Электронная почта")]
        [Required(ErrorMessage ="Введите email")]
        [DataType(DataType.EmailAddress)]
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
