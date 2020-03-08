using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OXG.ServiceCenterWeb.Models.SpecialModels
{
    public class AdminIndexModel
    {
        public List<string> Dates { get; set; } 

        public List<decimal> Moneys { get; set; }

        public List<string> Masters { get; set; }

        public List<decimal> Salaries { get; set; }

        public AdminIndexModel()
        {
            Dates = new List<string>();
            Moneys = new List<decimal>();
            Masters = new List<string>();
            Salaries = new List<decimal>();
        }
    }
}
