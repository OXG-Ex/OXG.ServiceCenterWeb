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

        public static List<Work> Works = new List<Work>()
        {
            new Work(){ Name ="Настройка ОС", Num=1,Price=1200},
            new Work(){ Name ="Чистка СО", Num=1,Price=450},
            new Work(){ Name ="Оптимизация ОС", Num=1,Price=500},
            new Work(){ Name ="Настройка ОС с сохр. данных", Num=1,Price=1500},
            new Work(){ Name ="Диагностика", Num=1,Price=400},
            new Work(){ Name ="Заправка картирджа HP, Canon черн. лазерн.", Num=1,Price=350},
            new Work(){ Name ="Заправка картирджа Samsung черн. лазерн.", Num=1,Price=400},
            new Work(){ Name ="Пересборка", Num=1,Price=450},
            new Work(){ Name ="Замена блока питания", Num=1,Price=450},
            new Work(){ Name ="Другое", Num=1,Price=0},
        };

        public static List<string> Statuses = new List<string>()
        {
            "Диагностика",
            "Ожидание согласования",
            "Ожидание комплектующих",
            "В работе",
            "Выдано"
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
