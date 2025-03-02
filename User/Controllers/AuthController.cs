using Common;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace User.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;

        public AuthController(JwtService jwtService, IConfiguration configuration)
        {
            _jwtService = jwtService;
            _configuration = configuration;
            _accountSid = _configuration["Twilio:AccountSid"];
            _authToken = _configuration["Twilio:AuthToken"];
            _fromPhoneNumber = _configuration["Twilio:FromPhoneNumber"];
        }

        /// <summary>
        /// Send OTP to user to validate.
        /// </summary>
        /// <param name="request">Phone number request.</param>
        /// <returns>Returns the jwt token.</returns>
        [HttpPost("send-otp")]
        public IActionResult SendOTP([FromBody] PhoneNumberRequest request)
        {
            try
            {
                // Validate user credentials (e.g., against a database)
                if (!string.IsNullOrEmpty(request.PhoneNumber))
                {
                    TwilioClient.Init(_accountSid, _authToken);

                    var verification = VerificationResource.Create(
                        to: "+917780509719",
                        channel: "sms",
                        pathServiceSid: "VA7cfc47d5fd3b8b52ad23847b44e4f934"
                        );

                    var token = _jwtService.GenerateToken(request.PhoneNumber);
                    return Ok(new { Token = token });
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Verify the user OTP.
        /// </summary>
        /// <param name="request">Phone number request.</param>
        /// <returns></returns>
        [HttpPost("verify-otp")]
        public IActionResult VerifyOTP([FromBody] PhoneNumberRequest request)
        {
            try
            {
                // Validate user credentials (e.g., against a database)
                if (!string.IsNullOrEmpty(request.OTP))
                {

                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}