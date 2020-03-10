using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OXG.ServiceCenterWeb.Models.SpecialModels
{
    /// <summary>
    /// Модель использующаяся для сохранения загруженных файлов
    /// </summary>
    public class FileModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
