using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteMVC.ViewModels
{
    public class User
    {
        [Required(ErrorMessage = "Введите Email пользователя")]
        [RegularExpression(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email имеет не правильный формат")]
        public string Email { get; set; }
        public string Login { get; set; }

        public bool IsSubPurchase { get; set; }

        [Required(ErrorMessage = "Введите пароль пользователя")]
        [DataType(DataType.Password)]
        [Compare("RepeatPassword", ErrorMessage = "Введенные пароли не совпадают")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Введите повторно пароль пользователя")]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }

        public string Phone { get; set; }
    }
}