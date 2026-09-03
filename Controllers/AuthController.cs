using Azure.Core;
using FirstProject.Data;
using FirstProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ADMIN_PKG _admin_pkg;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ADMIN_PKG admin_pkg, ILogger<AuthController> logger) { 
            this._admin_pkg= admin_pkg;
            this._logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Auth API is working!");
        }


        [HttpGet("my-profile/{adminId}")]
        [Authorize]
        async public Task<IActionResult> GetProfile(int adminId) {
            try
            {
                var profile=await this._admin_pkg.GetProfile(adminId);
                return StatusCode(200, new { success = true, profile });
            } catch (Exception ex) {

                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpPost("email-verify")]
        async public Task<IActionResult> VerifyEmail(Admin admin)
        {
            try
            {
                    await this._admin_pkg.AdminVerify(admin);
                    return StatusCode(200, new { success = true, message = "შეამოწმეთ მეილი" });

            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpPost("sign-up")]
        async public Task<IActionResult> AdminRegister(PasswordModel password,[FromQuery] string token)
        {
            try
            {
                    await this._admin_pkg.AdminRegister(password, token);
                    return StatusCode(200, new { success = true, message = "რეგისტრაცია წარმატებულია" });
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpPost("sign-in")]
        async public Task<IActionResult> AdminSignIn(AdminSignInRequest adminSignIn)
        {
            try
            {
                var admin = await this._admin_pkg.AdminSignIn(adminSignIn.Email, adminSignIn.Password);
                return StatusCode(200, new { success = true, Admin=admin.admin, token=admin.Token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }
    }
}
