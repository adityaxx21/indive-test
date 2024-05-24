using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using indive_test.Dtos.Account;
using indive_test.Dtos.Email;
using indive_test.Interfaces;
using indive_test.Models;
using indive_test.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace indive_test.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly IAccountRepository _accountRepository;

        private readonly ITokenService _tokenService;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly IEmailSender _emailSender;

        private readonly IConfiguration _configuration;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IEmailSender emailSender, IAccountRepository accountRepository, IConfiguration configuration)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginDto loginDto)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid username!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Account not found");

            return Ok(new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    TwoFactorEnabled = true
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        // Generate confirmation token
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

                        var encodedToken = WebUtility.UrlEncode(token);

                        var confirmationLink = Url.Action(
                            action: "ConfirmEmail",
                            null,
                            values: new { area = "Api", userid = appUser.Id, token },
                            protocol: Request.Scheme);

                        Console.WriteLine(confirmationLink);
                        // Store token to email verif
                        var accountVerif = new AppUserAccountVerification
                        {
                            Email = registerDto.Email,
                            VerifyToken = _tokenService.CreateRandomToken()
                        };

                        var account = await _accountRepository.CreateAsync(accountVerif);

                        // Send email verif

                        var emailSender = new EmailSendDto
                        {
                            Email = registerDto.Email,
                            Subject = "Confirm your email",
                            Message = $"<h1>Please confirm your email</h1><a href='{confirmationLink}'>Click the link to confirm your email.</a></p>"
                        };

                        await _emailSender.SendEmailAsync(emailSender.Email, emailSender.Subject, emailSender.Message);

                        // Send return message
                        return Ok("Succesfully");
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }

                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet("verify/email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            try
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return Ok("Success");
                }
                else
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new { Message = "Email confirmation failed", Errors = errors });
                }
            }
            catch (System.Exception e)
            {
                return BadRequest(e);
            }
        }


        // [HttpGet("google")]
        // public IActionResult GoogleLogin()
        // {
        //     var redirectUrl = "/api/account/google-response";
        //     var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        //     return Challenge(properties, "Google");
        // }

        // [HttpGet("google-response")]
        // public async Task<IActionResult> GoogleResponse()
        // {

        //     // var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //     // if (!authenticateResult.Succeeded)
        //     //     return BadRequest(); // Handle authentication failure

        //     // var claims = authenticateResult.Principal.Identities
        //     //     .FirstOrDefault()?.Claims
        //     //     .Select(claim => new
        //     //     {
        //     //         claim.Type,
        //     //         claim.Value
        //     //     });

        //     // return new JsonResult(claims);

        //     var result = await HttpContext.AuthenticateAsync("Google");

        //     var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new {
        //         claim.Issuer,
        //         claim.OriginalIssuer,
        //         claim.Type,
        //         claim.Value
        //     });

        //     return new JsonResult(claims);

        //     // var result = await HttpContext.AuthenticateAsync("Google");

        //     // if (!result.Succeeded)
        //     //     return BadRequest();

        //     // var claims = new[]
        //     // {
        //     //     new Claim(JwtRegisteredClaimNames.Sub, result.Principal.FindFirstValue(ClaimTypes.NameIdentifier)),
        //     //     new Claim(JwtRegisteredClaimNames.Email, result.Principal.FindFirstValue(ClaimTypes.Email)),
        //     //     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //     // };

        //     // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //     // var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //     // var token = new JwtSecurityToken(
        //     //     issuer: _configuration["Jwt:Issuer"],
        //     //     audience: _configuration["Jwt:Audience"],
        //     //     claims: claims,
        //     //     expires: DateTime.Now.AddMinutes(30),
        //     //     signingCredentials: creds);

        //     // return Ok(token);
        // }
    }
}