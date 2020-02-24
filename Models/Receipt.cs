using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OXG.ServiceCenterWeb.Models
{
    public class Receipt
    {
        public int Id { get; set; }

        [Display(Name = "Клиент")]
        public int ClientId { get; set; } = 1;
        [Display(Name = "Клиент")]
        public Client Client { get; set; } = new Client() { Id = 0, Name = "Empty User", Phone = "NO", DiscountNumber = 0 };

        [Display(Name = "Оборудование")]
        public int EquipmentId { get; set; } = 1;
        [Display(Name = "Обрудование")]
        public Equipment Equipment { get; set; } = new Equipment() { Id = 0, Name = "Default", Accesories = "" };

        [Display(Name = "Мастер")]
        public int EmployeerId { get; set; }
        [Display(Name = "Мастер")]
        public Employeer Employeer { get; set; }

        [Display(Name = "Заявленная неисправность")]
        [Required(ErrorMessage = "Введите неисправность", AllowEmptyStrings = false)]
        public string Malfunction { get; set; }

        [Display(Name = "Дата создания квитанции")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Дата закрытия квитанции")]
        [DataType(DataType.Date)]
        public DateTime ClosedDate { get; set; }

        [Display(Name = "Гарантия?")]
        public bool Warranty { get; set; } = false;

        [Display(Name = "Результат диагностики")]
        [DataType(DataType.MultilineText)]
        [UIHint("MultilineText")]
        public string DiagnosticResult { get; set; }/* = "";*/

        [Display(Name = "Оказанные услуги")]
        public List<Work> ServicesProvidet { get; set; }/* = new List<Work> {/* new Work() { Id = 0, Name = "NotPayment", Num = 0, Price = 0 } }*/

        [Display(Name = "Использованные компоненты")]
        public List<Component> Components { get; set; }/* = new List<Component> { };*/

        [Display(Name = "Статус квитанции")]
        [Required(ErrorMessage = "Выберите текущий статус", AllowEmptyStrings = false)]
        public string Status { get; set; }

        [Display(Name = "ИТОГО")]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }
    }
}
