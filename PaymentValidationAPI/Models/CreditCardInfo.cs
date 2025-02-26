﻿using System.ComponentModel.DataAnnotations;

namespace PaymentValidationAPI.Models
{
    public class CreditCardInfo
    {
        public string CardOwner { get; set; }
        public string CreditCardNumber { get; set; }
        [Required]
        public string IssueDate { get; set; }
        public string CVC { get; set; }

    }
}
