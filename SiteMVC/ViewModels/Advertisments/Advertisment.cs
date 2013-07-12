using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiteMVC.ViewModels.Advertisments
{
    public class Advertisment
    {
        [Required(ErrorMessage = "Не выбран раздел объявления.")]
        public int AdvertismentSection_Id { get; set; }
        public int AdvertismentSubSection_Id { get; set; }

        public string Address { get; set; }
        public string Phones { get; set; }

        [Required(ErrorMessage = "Текст объявления не может быть пустым.")]
        public string Text { get; set; }
    }
}