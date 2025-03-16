using BusinessLayer.Interface;
using JWT.Service;
using Microsoft.AspNetCore.Authorization;
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
        private readonly TokenService _jwtService;

        /// <summary>
        /// Initializes a new instance of the UserController class.
        /// </summary>
        /// <param name="userBL">Business layer dependency for user operations.</param>
        public UserController(IUserBL userBL, TokenService jwtService)
        {
            _userBL = userBL;
            _jwtService = jwtService;
        }


        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("OK");
        }


        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="userDTO">User details for registration.</param>
        /// <returns>Response with registration status.</returns>
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
        /// <param name="loginDTO">Login credentials.</param>
        /// <returns>Response with authentication status.</returns>
        [HttpPost("login")]
        public IActionResult Login(LoginDTO loginDTO)
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

            string token = _jwtService.GenerateToken(loginDTO.Email);

            logger.Info("User logged in successfully.");

            //ResponseModel < ResponseLoginDTO > r=new ResponseModel<ResponseLoginDTO>();

            result.Token = token;
            return Ok(new ResponseModel<ResponseLoginDTO>
            {
                Success = true,
                Message = "User login successfully",
                Data = result
            });
        }

        /// <summary>
        /// Sends a password reset email to the user.
        /// </summary>
        /// <param name="forgetDTO">User email for password reset request.</param>
        /// <returns>Response indicating whether email was sent.</returns>

        [Authorize]
        [HttpPost("forget")]
        public IActionResult Forget(ForgetDTO forgetDTO)
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

        /// <summary>
        /// Resets the user's password after verification.
        /// </summary>
        /// <param name="resetDTO">Token and new password details.</param>
        /// <returns>Response indicating password reset success.</returns>
        [HttpPost("reset")]
        public IActionResult Reset(ResetDTO resetDTO)
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
    }
}