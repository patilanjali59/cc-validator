using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PaymentValidationAPI.Models;


namespace PaymentValidationAPI.Services
{
    public class CreditCardValidator
    {
        private static readonly Dictionary<string, Regex> CardPatterns = new()
        {
            { "Visa", new Regex("^4[0-9]{12}(?:[0-9]{3})?$", RegexOptions.Compiled) },
            { "MasterCard", new Regex("^5[1-5][0-9]{14}$", RegexOptions.Compiled) },
            { "American Express", new Regex("^3[47][0-9]{13}$", RegexOptions.Compiled) }
        };

        public ValidationResult Validate(CreditCardInfo cardInfo)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(cardInfo.CardOwner))
                errors.Add("Card owner is required.");

             string pattern = @"^[a-zA-Z ]+$";
            if (!Regex.IsMatch(cardInfo.CardOwner, pattern)) 
                errors.Add("Card owner name contains invalid characters. Invalid card owner name");

            var cardType = GetCardType(cardInfo.CreditCardNumber);
            if (string.IsNullOrEmpty(cardType))
                errors.Add("Invalid credit card number.");

            string date = @"^(0[1-9]|1[0-2])/([0-9]{4})$";
            if (!Regex.IsMatch(cardInfo.IssueDate, date))
                errors.Add("Credit card date is invalid.");

            string[] ccDate = cardInfo.IssueDate.Split('/');
            if (((UInt16.Parse(ccDate[1]) + 3) < DateTime.Now.Year) || (UInt16.Parse(ccDate[1]) > DateTime.Now.Year))
                errors.Add("Invalid Issue Date.");

            if (!IsValidCVC(cardInfo.CVC, cardType))
                errors.Add("Invalid CVC.");

            if (errors.Any())
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Errors = errors.ToArray()
                };
            }

            return new ValidationResult
            {
                IsValid = true,
                CreditCardType = cardType
            };
        }

        private string GetCardType(string cardNumber)
        {
            return CardPatterns.FirstOrDefault(pattern => pattern.Value.IsMatch(cardNumber)).Key;
        }

        private bool IsValidCVC(string cvc, string cardType)
        {
            return cardType switch
            {
                "American Express" => Regex.IsMatch(cvc, @"^[0-9]{4}$"),
                _ => Regex.IsMatch(cvc, @"^[0-9]{3}$"),
            };
        }
    }
}
