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
        public Guid AdvertismentSection_Id { get; set; }
        public Guid AdvertismentSubSection_Id { get; set; }

        public string Address { get; set; }
        public string Phones { get; set; }

        [Required(ErrorMessage = "Текст объявления не может быть пустым.")]
        public string Text { get; set; }
    }

    [Serializable]
    public class AdvertismentPhotoResponse
    {
        public string Name { get; set; }
        public int Size { get; set; }

        public string Url { get; set; }
        public string Thumbnail_url { get; set; }

        public string Delete_url { get; set; }
        public string Delete_type { get; set; }
    }
}