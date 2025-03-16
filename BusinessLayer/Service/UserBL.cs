using System;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using RepositoryLayer.Entity;
using NLog;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Handles business logic for user-related operations.
    /// </summary>
    public class UserBL : IUserBL
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserRL _userRL;

        /// <summary>
        /// Initializes a new instance of the UserBL class.
        /// </summary>
        /// <param name="userRL">Repository layer dependency for user operations.</param>
        public UserBL(IUserRL userRL)
        {
            _userRL = userRL;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userDTO">User details for registration.</param>
        /// <returns>Registered user entity or null if already exists.</returns>
        public User RegisterBL(UserDTO userDTO)
        {
            try
            {
                logger.Info("Attempting user registration for email: {0}", userDTO.Email);
                var result = _userRL.RegisterRL(userDTO);

                if (result == null)
                {
                    logger.Warn("User registration failed. Email already exists: {0}", userDTO.Email);
                    return null;
                }

                logger.Info("User registered successfully: {0}", userDTO.Email);
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred during user registration for email: {0}", userDTO.Email);
                throw new Exception("An error occurred while registering the user.");
            }
        }

        /// <summary>
        /// Handles user login by verifying credentials.
        /// </summary>
        /// <param name="loginDTO">Login credentials.</param>
        /// <returns>ResponseLoginDTO with user details if successful; otherwise, null.</returns>
        public ResponseLoginDTO LoginBL(LoginDTO loginDTO)
        {
            try
            {
                logger.Info("User login attempt for email: {0}", loginDTO.Email);
                var result = _userRL.LoginRL(loginDTO);

                if (result == null)
                {
                    logger.Warn("Login failed. Credentials not found: {0}", loginDTO.Email);
                    return null;
                }

                logger.Info("User login successful: {0}", loginDTO.Email);
                return new ResponseLoginDTO
                {
                    Email = result.Email,
                    FirstName = result.FirstName,
                    LastName = result.LastName
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred during user login for email: {0}", loginDTO.Email);
                throw new Exception("An error occurred while logging in.");
            }
        }

        /// <summary>
        /// Handles forgot password requests.
        /// </summary>
        /// <param name="forgetDTO">User email for password recovery.</param>
        /// <returns>True if user exists; otherwise, false.</returns>
        public bool ForgetBL(ForgetDTO forgetDTO)
        {
            try
            {
                logger.Info("Password reset request for email: {0}", forgetDTO.Email);
                bool result = _userRL.ForgetRL(forgetDTO);

                if (!result)
                {
                    logger.Warn("Password reset failed. User not found: {0}", forgetDTO.Email);
                    return false;
                }

                logger.Info("Password reset request successful for email: {0}", forgetDTO.Email);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred during password reset request for email: {0}", forgetDTO.Email);
                throw new Exception("An error occurred while processing the password reset request.");
            }
        }
    }
}
