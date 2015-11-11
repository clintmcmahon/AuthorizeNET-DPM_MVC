using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AuthorizeNET_DPM_MVC.Models
{
    public class DPMModel
    {
        public string FPHash {get;set;}
        public string FPSequence {get;set;}
        public string InvoiceNumber {get;set;}
        public string FPTimeStamp {get;set;}
        public string Login {get;set;}
        public string PostBackUrl { get; set; }
        public string RelayUrl {get;set;}
        public string RelayResponse {get;set;}
        public string ReceiptLinkUrl {get;set;}
        public string TestRequest {get;set;}
        public string Method {get;set;}
        public string Email { get; set; }
     
        [Required]
        [Display(Name="Cardholder First Name")]
        public string x_first_name {get;set;}

        [Required]
        [Display(Name = "Cardholder First Name")]
        public string x_last_name {get;set;}

        public string x_cust_id { get; set; }
        public string x_address {get;set;}
        public string x_city {get;set;}
        public string x_state {get;set;}

        [Required]
        [Display(Name = "Cardholder Phone Number")]
        public string x_phone { get; set; }

        [Required]
        [Display(Name="Billing Zip Code")]
        public string x_zip {get;set;}
        public decimal Amount {get;set;}

        [Required]
        [Display(Name = "Credit Card Number")]
        public string x_card_num { get; set; }

        [Required]
        [Display(Name = "Card Expiration Date")]
        public string x_exp_date { get; set; }

        [Required]
        [Display(Name = "Card CCV Code")]
        public string x_card_code { get; set; }
    }
}
