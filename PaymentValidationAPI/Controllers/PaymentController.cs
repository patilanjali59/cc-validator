using Microsoft.AspNetCore.Mvc;
using PaymentValidationAPI.Models;
using PaymentValidationAPI.Services;

namespace PaymentValidationAPI.Controllers
{
    
        [Route("api/[controller]")]
        [ApiController]
        public class PaymentController : ControllerBase
        {
            private readonly CreditCardValidator _validator;

            public PaymentController(CreditCardValidator validator)
            {
                _validator = validator;
            }

            [HttpPost("validate")]
            public IActionResult Validate([FromBody] CreditCardInfo creditCardInfo)
            {
            if (creditCardInfo == null)
                return BadRequest(new ValidationResult
                {
                    IsValid = false,
                    Errors = new[] { "Invalid input data." }
                });

                var result = _validator.Validate(creditCardInfo);
                return result.IsValid ? Ok(result) : BadRequest(result);
            }
        }
}

