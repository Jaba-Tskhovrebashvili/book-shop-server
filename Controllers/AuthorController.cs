using FirstProject.Data;
using FirstProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace FirstProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        public AUTHOR_PKG _author_pkg;
        private readonly ILogger<AuthorController> _logger;
        public AuthorController(AUTHOR_PKG author_pkg, ILogger<AuthorController> logger) {
         this._author_pkg = author_pkg;
         this._logger = logger;
        }

        [HttpGet("get-authors")]
        public async Task<IActionResult> GetAuthors(string? search,int? cityId,int? countryId,int? sexId, int page)
        {
            try
            {
                
                var authors = await this._author_pkg.GetAuthors(search,cityId,countryId, sexId, page);
                return StatusCode(200, new { success = true, authors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpPost("add-author")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAuthor(AddAuthor author)
        {
            try
            {
                var newAuthor = await this._author_pkg.AddAuthor(author);
                return StatusCode(200, new { success = newAuthor, message="ავტორი დაემატა წარმატებით" });
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpPut("edit-author/{authorId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAuthor(AddAuthor author, int authorId)
        {
            try
            {
                var editAuthor = await this._author_pkg.EditAuthor(author, authorId);
                return StatusCode(200, new { success = editAuthor,message= "ავტორის მონაცემები შეიცვალა" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpDelete("delete-author/{authorId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            try
            {
                var DeleteAuthor = await this._author_pkg.DeleteAuthor(authorId);
                return StatusCode(200, new { success = DeleteAuthor, message="ავტორის ექაუნთი წაიშალა წარმატებით" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }


        [HttpGet("get-cities")]
        public async Task<IActionResult> GetCities(string? search, int page)
        {
            try
            {

                var cities = await this._author_pkg.GetCities(search, page);
                return StatusCode(200, new { success = true, cities });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpGet("get-countries")]
        public async Task<IActionResult> GetCountries(string? search, int page)
        {
            try
            {

                var countries = await this._author_pkg.GetCountries(search, page);
                return StatusCode(200, new { success = true, countries });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpGet("author-sex")]
        public async Task<IActionResult> GetSex()
        {
            try
            {

                var authorSex = await this._author_pkg.GetAuthorSex();
                return StatusCode(200, new { success = true, authorSex });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

    }
}
