namespace Common.Models
{
    /// <summary>
    /// Phone number request class.
    /// </summary>
    public class PhoneNumberRequest
    {
        /// <summary>
        /// Get and set phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Get and set OTP.
        /// </summary>
        public string OTP { get; set; }
    }
}