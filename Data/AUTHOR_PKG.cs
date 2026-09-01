using FirstProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using FirstProject.Helper;

namespace FirstProject.Data
{
    public class AUTHOR_PKG
    {
        private readonly AddDbContext _context;
        private readonly AuthorHelper _authorHelper;
        public AUTHOR_PKG(AddDbContext context, AuthorHelper authorHelper) {
            this._context = context;
            this._authorHelper= authorHelper;
        }

        public async Task<bool> AddAuthor(AddAuthor author)
        {
           
            try
            {

                var findUser = await this._context.author.FirstOrDefaultAsync(x => x.Email == author.Email);
                if (findUser != null)
                {
                    throw new Exception("მსგავსი მეილით ექაუნთი უკვე არსებობს");

                }
                var authorValidate =await this._authorHelper.AuthorValidate(author);

                var newAuth = new Author
                {
                    Name = authorValidate.Name,
                    Surname = authorValidate.Surname,
                    SexId = authorValidate.SexId,
                    PersonalNumber= authorValidate.PersonalNumber,
                    BirthDate= authorValidate.BirthDate.Date,
                    CountryId= authorValidate.CountryId,
                    CityId= authorValidate.CityId,
                    PhoneNumber= authorValidate.PhoneNumber,
                    Email= authorValidate.Email,
                    Created_At = DateTime.Now,
                    Updated_At = DateTime.Now,
                };

                await _context.author.AddAsync(newAuth);

                await _context.SaveChangesAsync();

                return true;

            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> EditAuthor(AddAuthor author,int authorId)
        {
            try
            {
                var findAuthor= await this._context.author.FirstOrDefaultAsync(x=>x.Id==authorId);

                if (findAuthor == null)
                {
                    throw new Exception("დაფიქსირდა შეცდომა");

                }

                var authorValidate = await this._authorHelper.AuthorValidate(author);

                findAuthor.Name = authorValidate.Name;
                findAuthor.Surname = authorValidate.Surname;
                findAuthor.SexId = authorValidate.SexId;
                findAuthor.PersonalNumber = authorValidate.PersonalNumber;
                findAuthor.BirthDate = authorValidate.BirthDate.Date;
                findAuthor.CountryId = authorValidate.CountryId;
                findAuthor.CityId = authorValidate.CityId;
                findAuthor.PhoneNumber = authorValidate.PhoneNumber;
                findAuthor.Email = authorValidate.Email;
                findAuthor.Updated_At = DateTime.Now;

                await _context.SaveChangesAsync();

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAuthor(int authorId)
        {
            try
            {
                var findAuthor = await this._context.author
                    .Include(x=>x.Products)
                    .FirstOrDefaultAsync(x => x.Id == authorId);

                if (findAuthor == null)
                {
                    throw new Exception("დაფიქსირდა შეცდომა");

                }

                foreach (var product in findAuthor.Products)
                {
                    var count = await this._context.product
                    .Where(x => x.Id == product.Id)
                    .SelectMany(x => x.Authors)
                    .CountAsync();

                    if (count <= 1)
                    {
                        _context.product.Remove(product);
                    }
                }

                _context.author.Remove(findAuthor);
                await _context.SaveChangesAsync();

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
