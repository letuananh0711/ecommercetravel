﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace Travel_G08.Areas.Admin.Models
{
    public class TaiKhoan : IValidatableObject
    {
        [Required(ErrorMessage = "Please enter the email.")]
        public string email { get; set; }
        [Required(ErrorMessage = "Please enter the password.")]
        public string password { get;set;}
        [Required(ErrorMessage = "Please confirm the password.")]
        public string confirm_password { get; set; }
        public string status { get; set; }
        //public ValueProviderResult Value { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (password != confirm_password)
            {
                yield return new ValidationResult("Confirm password do not match");
            }
        }
    }
}