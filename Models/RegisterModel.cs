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
        [Required(ErrorMessage ="Данное поле обязательно для заполения")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"(?i)\b[A-Z0-9._%-]+@[A-Z0-9.-]+\[A-Z]{2,4}\b",ErrorMessage ="Неверный Email")]
        public string Email { get; set; }

        [Display(Name = "Введите пароль")]
        [Required(ErrorMessage = "Данное поле обязательно для заполения")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Подтвердите пароль")]
        [Required(ErrorMessage = "Данное поле обязательно для заполения")]
        [DataType(DataType.Password)]
     //   [Compare("Password",ErrorMessage ="Пароли не совпадают")]
        public string ConfirmPasswod { get; set; }
    }
}
