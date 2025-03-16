using BusinessLayer.Interface;
using JWT.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using Middleware.GlobalExceptionHandler;

namespace HelloGreetingApplication.Controllers
{
    /// <summary>
    /// Handles user-related operations such as registration, login, password reset, and email notifications.
    /// </summary>
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserBL _userBL;
        private readonly TokenService _jwtService;
        private readonly EmailService _emailService;

        public UserController(IUserBL userBL, TokenService jwtService, EmailService emailService)
        {
            _userBL = userBL;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok("OK");
        //}

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                logger.Warn("Validation error in Register request.");
                return BadRequest(ModelState);
            }

            try
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
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in Register");
                return BadRequest(ExceptionHandler.CreateErrorResponse(ex));
            }
        }

        /// <summary>
        /// Authenticates a user and logs them in.
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                logger.Warn("Validation error in Login request.");
                return BadRequest(ModelState);
            }

            try
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

                string token = _jwtService.GenerateToken(loginDTO.Id, loginDTO.Email);
                result.Token = token;

                logger.Info("User logged in successfully.");
                return Ok(new ResponseModel<ResponseLoginDTO>
                {
                    Success = true,
                    Message = "User login successful",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in Login");
                return BadRequest(ExceptionHandler.CreateErrorResponse(ex));
            }
        }

        /// <summary>
        /// Sends a password reset email to the user.
        /// </summary>
        [HttpPost("forget")]
        public IActionResult Forget([FromBody] ForgetDTO forgetDTO)
        {
            if (!ModelState.IsValid)
            {
                logger.Warn("Validation error in Forget request.");
                return BadRequest(ModelState);
            }

            try
            {
                logger.Info($"Password reset request received for email: {forgetDTO.Email}");

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
                logger.Error(ex, "Exception in Forget");
                return BadRequest(ExceptionHandler.CreateErrorResponse(ex));
            }
        }

        /// <summary>
        /// Resets the user's password after verification.
        /// </summary>
        [HttpPost("reset")]
        public IActionResult Reset([FromBody] ResetDTO resetDTO)
        {
            try
            {
                string token = HttpContext.Request.Query["token"];
                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(resetDTO.Password))
                {
                    return BadRequest(new { message = "Token and new password are required." });
                }

                var email = _jwtService.ValidateResetToken(token);
                if (email == null)
                {
                    return Unauthorized(new { message = "Invalid or expired token." });
                }

                bool updateSuccess = _userBL.UpdateUserPassword(email, resetDTO);
                if (!updateSuccess)
                {
                    return BadRequest(new { message = "Failed to update password." });
                }

                logger.Info("Password changed successfully.");
                return Ok(new { message = "Password changed successfully!" });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in Reset");
                return BadRequest(ExceptionHandler.CreateErrorResponse(ex));
            }
        }
    }
}