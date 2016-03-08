﻿using System.ComponentModel.DataAnnotations;

namespace GasPrice.Core.ViewModels.UserAccount
{
    public class ChangePasswordInputModel
    {
        //[Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        //[Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        //[Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string NewPasswordConfirm { get; set; }
    }
}