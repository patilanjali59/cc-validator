using PaymentValidationAPI.Models;
using PaymentValidationAPI.Services;


namespace PaymentValidationAPI.Tests
{
    public class CreditCardValidatorTests
    {
        private readonly CreditCardValidator _validator;

        public CreditCardValidatorTests()
        {
            _validator = new CreditCardValidator();
        }

        [Test]
        public void Validate_ShouldReturnErrors_WhenCardOwnerIsEmpty()
        {
            var cardInfo = new CreditCardInfo
            {
                CardOwner = "",
                CreditCardNumber = "4111111111111111",
                IssueDate = "01/2022",
                CVC = "123"
            };

            var result = _validator.Validate(cardInfo);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors, Does.Contain("Card owner is required."));
            });
        }

        [Test]
        public void Validate_ShouldReturnErrors_WhenCardOwnerNameHasDigit()
        {
            var cardInfo = new CreditCardInfo
            {
                CardOwner = "John2Doe",
                CreditCardNumber = "4111111111111111",
                IssueDate = "01/2022",
                CVC = "123"
            };

            var result = _validator.Validate(cardInfo);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors, Does.Contain("Card owner name contains invalid characters. Invalid card owner name"));
            });
        }

        [Test]
        public void Validate_ShouldReturnErrors_WhenCardNumberIsInvalid()
        {
            var cardInfo = new CreditCardInfo
            {
                CardOwner = "John Doe",
                CreditCardNumber = "1234567890123456",
                IssueDate = "01/2022",
                CVC = "123"
            };

            var result = _validator.Validate(cardInfo);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors, Does.Contain("Invalid credit card number."));
            });
        }

        [Test]
        public void Validate_ShouldReturnErrors_WhenCardDateIsInvalid()
        {
            var cardInfo = new CreditCardInfo
            {
                CardOwner = "John Doe",
                CreditCardNumber = "4111111111111111",
                IssueDate = "16/2066",
                CVC = "123"
            };

            var result = _validator.Validate(cardInfo);

            Assert.False(result.IsValid);
            Assert.Contains("Credit card date is invalid.", result.Errors);
        }


        [Test]
        public void Validate_ShouldReturnErrors_WhenInvalidIssueDate()
        {
            var cardInfo = new CreditCardInfo
            {
                CardOwner = "John Doe",
                CreditCardNumber = "4111111111111111",
                IssueDate = "01/2026",
                CVC = "123"
            };

            var result = _validator.Validate(cardInfo);

            Assert.False(result.IsValid);
            Assert.Contains("Invalid Issue Date.", result.Errors);
        }

        [Test]
        public void Validate_ShouldReturnErrors_WhenCVCIsInvalid()
        {
            var cardInfo = new CreditCardInfo
            {
                CardOwner = "John Doe",
                CreditCardNumber = "4111111111111111",
                IssueDate = "01/2022",
                CVC = "12"
            };

            var result = _validator.Validate(cardInfo);

            Assert.False(result.IsValid);
            Assert.Contains("Invalid CVC.", result.Errors);
        }

        [Test]
        public void Validate_ShouldReturnSuccess_WhenCardInfoIsValid()
        {
            var cardInfo = new CreditCardInfo
            {
                CardOwner = "John Doe",
                CreditCardNumber = "4111111111111111",
                IssueDate = "01/2022",
                CVC = "123"
            };

            var result = _validator.Validate(cardInfo);

            Assert.That(result.IsValid, Is.True);
            Assert.That("Visa", Is.EqualTo(result.CreditCardType));
        }
    }
}
