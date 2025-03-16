using System;
using System.Linq;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using NLog;
using Security;

namespace RepositoryLayer.Service
{
    /// <summary>
    /// Handles user-related database operations such as registration, login, and password recovery.
    /// </summary>
    public class UserRL : IUserRL
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UserContext _context;

        /// <summary>
        /// Initializes a new instance of the UserRL class.
        /// </summary>
        /// <param name="context">Database context for user entity.</param>
        public UserRL(UserContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Registers a new user if the email is not already in use.
        /// </summary>
        /// <param name="userDTO">Data transfer object containing user details.</param>
        /// <returns>Registered user entity if successful; otherwise, null.</returns>
        public User RegisterRL(UserDTO userDTO)
        {
            try
            {
                logger.Info("Attempting to register new user with email: {0}", userDTO.Email);

                var existingUser = _context.Users.FirstOrDefault(e => e.Email == userDTO.Email);
                if (existingUser == null)
                {
                    var user = new User
                    {
                        Email = userDTO.Email,
                        Password = PasswordHelper.HashPassword(userDTO.Password),
                        FirstName = userDTO.FirstName,
                        LastName = userDTO.LastName,
                    };

                    _context.Users.Add(user);
                    _context.SaveChanges();

                    logger.Info("User registered successfully with email: {0}", userDTO.Email);
                    return user;
                }

                logger.Warn("Registration failed: Email {0} is already in use.", userDTO.Email);
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred during user registration for email: {0}", userDTO.Email);
                throw new Exception("An error occurred while registering the user.");
            }
        }

        /// <summary>
        /// Authenticates a user based on email and password.
        /// </summary>
        /// <param name="loginDTO">Data transfer object containing login credentials.</param>
        /// <returns>User entity if credentials are valid; otherwise, null.</returns>
        public User LoginRL(LoginDTO loginDTO)
        {
            try
            {
                logger.Info("User login attempt with email: {0}", loginDTO.Email);

                var user = _context.Users.FirstOrDefault(e => e.Email == loginDTO.Email);
                if (user == null)
                {
                    logger.Warn("Login failed: No user found with email {0}", loginDTO.Email);
                    return null;
                }

                if (PasswordHelper.VerifyPassword(loginDTO.Password, user.Password))
                {
                    logger.Info("Login successful for email: {0}", loginDTO.Email);
                    return user;
                }

                logger.Warn("Login failed: Credentials not matched for email {0}", loginDTO.Email);
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred during user login for email: {0}", loginDTO.Email);
                throw new Exception("An error occurred while logging in.");
            }
        }

        /// <summary>
        /// Checks if a user exists with the provided email for password reset purposes.
        /// </summary>
        /// <param name="forgetDTO">Data transfer object containing the email.</param>
        /// <returns>True if user exists; otherwise, false.</returns>
        public bool ForgetRL(ForgetDTO forgetDTO)
        {
            try
            {
                logger.Info("Password reset request received for email: {0}", forgetDTO.Email);

                var userExists = _context.Users.Any(e => e.Email == forgetDTO.Email);
                if (!userExists)
                {
                    logger.Warn("Password reset failed: No user found with email {0}", forgetDTO.Email);
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
