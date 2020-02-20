using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OXG.ServiceCenterWeb.Models.SpecialModels
{
    public static class StaticValues
    {
        public static List<string> EmployeerRoles = new List<string>() 
        {
            "Администратор",
            "Директор",
            "Мастер",
            "Приемщик"
        };

        public static List<string> MasterSpecializations = new List<string>()
        {
            "Ремонт ПК",
            "Ремонт мобильных устройств",
            "Ремонт орг. техники",
            "Заправка картриджей",
            "Ремонт бытовой техники",
            "Компонентный ремонт"
        };

        public static Role Admin = new Role() { Name = "Администратор" };

        public static Role Director = new Role() { Name = "Директор" };

        public static Role Master = new Role() { Name = "Мастер" };

        public static Role Receiver = new Role() { Name = "Приёмщик" };
    }
}
