using ElectroEshop.CustomCode;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElectroEshop.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        [Required(ErrorMessage = " You've to accept the terms & conditions")]
        [Display(Name = "I've read and accept the terms & conditions")]
        public string ConditionsBool { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class CheckoutViewModel
    {
        [Required]
        [Display(Name = "You are logged in, use my account details.")]
        public bool UserAccountBool { get; set; }

        [RequiredIf("UserAccountBool", Operator.EqualTo, false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [RequiredIf("UserAccountBool", Operator.EqualTo, false)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [RequiredIf("UserAccountBool", Operator.EqualTo, false)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [RequiredIf("UserAccountBool", Operator.EqualTo, false)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [RequiredIf("UserAccountBool", Operator.EqualTo, false)]
        [Display(Name = "City")]
        public string City { get; set; }

        [RequiredIf("UserAccountBool", Operator.EqualTo, false)]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [RequiredIf("UserAccountBool", Operator.EqualTo, false)]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [RequiredIf("UserAccountBool", Operator.EqualTo, false)]
        [Display(Name = "Telephone")]
        public string Telephone { get; set; }

        [Display(Name = "Create Account?")]
        public bool CreateAccountBool { get; set; }

        [RequiredIf("CreateAccountBool", Operator.EqualTo, true)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        // Additional Shipping Address

        //[Required]
        //[Display(Name = "Ship to a different address?")]
        //public bool DiifferentAddressBool { get; set; }

        [Required]
        [Display(Name = "Ship to a different address?")]
        public bool DifferentAddressBool { get; set; }

        [RequiredIf("DifferentAddressBool", Operator.EqualTo, true)]
        [Display(Name = "First Name")]
        public string SPfirstName { get; set; }

        [RequiredIf("DifferentAddressBool", Operator.EqualTo, true)]
        [Display(Name = "Last Name")]
        public string SPlastName { get; set; }

        [RequiredIf("DifferentAddressBool", Operator.EqualTo, true)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string SPemail { get; set; }

        [RequiredIf("DifferentAddressBool", Operator.EqualTo, true)]
        [Display(Name = "Address")]
        public string SPaddress { get; set; }

        [RequiredIf("DifferentAddressBool", Operator.EqualTo, true)]
        [Display(Name = "City")]
        public string SPcity { get; set; }

        [RequiredIf("DifferentAddressBool", Operator.EqualTo, true)]
        [Display(Name = "Country")]
        public string SPcountry { get; set; }

        [RequiredIf("DifferentAddressBool", Operator.EqualTo, true)]
        [Display(Name = "Zip Code")]
        public string SPzipCode { get; set; }

        [RequiredIf("DifferentAddressBool", Operator.EqualTo, true)]
        [Display(Name = "Telephone")]
        public string SPtelephone { get; set; }

        //[Display(Name = "I've read and accept the terms & conditions")]
        //public string ConditionsBool { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }
    }
}
