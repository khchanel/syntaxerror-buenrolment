using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace BUEnrolment.Models
{
    /// <summary>
    /// Login authentication model
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// User name
        /// </summary>
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// boolean flag for Remember me option
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// Register model for authentication
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Store all user roles
        /// </summary>
        public IEnumerable<SelectListItem> items
        {
            get
            {
                foreach (var role in Roles.GetAllRoles())
                {
                    yield return new SelectListItem {Text = role.ToString(), Value = role.ToString()};
                }
            }
        }

        /// <summary>
        /// User name
        /// </summary>
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        /// <summary>
        /// User full name
        /// </summary>
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Confirm password
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// User role
        /// </summary>
        [Display(Name = "Role")]
        public string Role { get; set; }
    }

}
