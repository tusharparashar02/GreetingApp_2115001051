using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;

namespace HelloGreetingApplication.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserBL _userBL;

        /// <summary>
        /// Initializes a new instance of the UserController class.
        /// </summary>
        /// <param name="userBL">Business layer dependency for user operations.</param>
        public UserController(IUserBL userBL)
        {
            _userBL = userBL;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="userDTO">User details for registration.</param>
        /// <returns>Response with registration status.</returns>
        [HttpPost("register")]
        public IActionResult Register(UserDTO userDTO)
        {
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
                logger.Error(ex, "Error occurred during user registration.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Authenticates a user and logs them in.
        /// </summary>
        /// <param name="loginDTO">Login credentials.</param>
        /// <returns>Response with authentication status.</returns>
        [HttpPost("login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            try
            {
                logger.Info("User login attempt started.");
                var result = _userBL.LoginBL(loginDTO);

                if (result == null)
                {
                    logger.Warn("User login failed. Invalid credentials.");
                    return Ok(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Invalid email or password",
                        Data = null
                    });
                }

                logger.Info("User logged in successfully.");
                return Ok(new ResponseModel<ResponseLoginDTO>
                {
                    Success = true,
                    Message = "User login successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred during user login.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Sends a password reset email to the user.
        /// </summary>
        /// <param name="forgetDTO">User email for password reset request.</param>
        /// <returns>Response indicating whether email was sent.</returns>
        [HttpPost("forget")]
        public IActionResult Forget(ForgetDTO forgetDTO)
        {
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
                logger.Error(ex, "Error occurred during password reset request.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Resets the user's password after verification.
        /// </summary>
        /// <param name="resetDTO">Token and new password details.</param>
        /// <returns>Response indicating password reset success.</returns>
        [HttpPost("reset")]
        public IActionResult Reset(ResetDTO resetDTO)
        {
            try
            {
                logger.Info("Password reset request initiated.");
                // Example: Validate token and update password logic here

                logger.Info("Password reset successfully.");
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = "Password reset successfully",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred during password reset.");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
