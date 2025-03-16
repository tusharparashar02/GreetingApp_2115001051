using BusinessLayer.Interface;
using JWT.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using Org.BouncyCastle.Asn1.Ocsp;

namespace HelloGreetingApplication.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserBL _userBL;
        private readonly TokenService _jwtService;
        private readonly EmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the UserController class.
        /// </summary>
        public UserController(IUserBL userBL, TokenService jwtService, EmailService emailService)
        {
            _userBL = userBL;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("OK");
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        [HttpPost("register")]
        public IActionResult Register(UserDTO userDTO)
        {
            logger.Info("User registration process started.");
            var result = _userBL.RegisterBL(userDTO);

            if (result == null)
            {
                logger.Warn("User already exists.");
                return Ok(new ResponseModel<object>
                {
                    Success = false,
                    Message = "User already present",
                    Data = null
                });
            }

            logger.Info("User registered successfully.");
            return Ok(new ResponseModel<object>
            {
                Success = true,
                Message = "User registered",
                Data = result
            });
        }

        /// <summary>
        /// Authenticates a user and logs them in.
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            logger.Info("User login attempt started.");
            var result = _userBL.LoginBL(loginDTO);

            if (result == null)
            {
                logger.Warn("Invalid login credentials.");
                return Ok(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Invalid email or password",
                    Data = null
                });
            }

            string token = _jwtService.GenerateToken(loginDTO.Email);
            result.Token = token;

            logger.Info("User logged in successfully.");
            return Ok(new ResponseModel<ResponseLoginDTO>
            {
                Success = true,
                Message = "User login successful",
                Data = result
            });
        }

        /// <summary>
        /// Sends a password reset email to the user.
        /// </summary>
        [HttpPost("forget")]
        public IActionResult Forget(ForgetDTO forgetDTO)
        {
            logger.Info($"Password reset request received for email: {forgetDTO.Email}");

            if (string.IsNullOrEmpty(forgetDTO.Email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            var result = _userBL.ForgetBL(forgetDTO);

            if (!result)
            {
                logger.Warn("User not found for password reset.");
                return Ok(new ResponseModel<object>
                {
                    Success = false,
                    Message = "No user found with this email",
                    Data = null
                });
            }

            string resetToken = _jwtService.GenerateResetToken(forgetDTO.Email);
            string resetLink = $"https://localhost:7277/api/user/reset?token={resetToken}";

            // Send email synchronously
            try
            {
                _emailService.SendEmail(forgetDTO.Email, "Reset Your Password",
                    $"Click the link to reset your password: <a href='{resetLink}'>Reset Password</a>");

                logger.Info("Password reset email sent successfully.");
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = "Password reset email sent successfully",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                logger.Error($"Error sending email: {ex.Message}");
                return StatusCode(500, new ResponseModel<object>
                {
                    Success = false,
                    Message = "Failed to send password reset email",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Resets the user's password after verification.
        /// </summary>
        [HttpPost("reset")]
        public IActionResult Reset(ResetDTO resetDTO)
        {
            try
            {
                // Manually get token from query parameters
                string token = HttpContext.Request.Query["token"];

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(resetDTO.Password))
                {
                    return BadRequest(new { message = "Token and new password are required." });
                }

                // Validate the token
                var email = _jwtService.ValidateResetToken(token);
                if (email == null)
                {
                    return Unauthorized(new { message = "Invalid or expired token." });
                }

                // Update the password in the database
                bool updateSuccess = _userBL.UpdateUserPassword(email, resetDTO);
                if (!updateSuccess)
                {
                    return BadRequest(new { message = "Failed to update password." });
                }

                return Ok(new { message = "Password changed successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error occurred while resetting password", error = ex.Message });
            }
        }
    }
}
