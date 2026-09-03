using FirstProject.Helper;
using FirstProject.Models;
using FirstProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace FirstProject.Data
{
    public class ADMIN_PKG 
    {

        private readonly AddDbContext _context;

        private AuthHelper _authHelper;
        private EmailService _emailservice;
        public ADMIN_PKG( AuthHelper authHelper, EmailService emailservice, AddDbContext context)  {
            this._authHelper = authHelper;
            this._emailservice = emailservice;
            _context = context;
        }


        public static bool IsValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        public async Task<Admin> GetProfile(int id)
        {
            try
            {
                var findAdmin=await this._context.admin.FirstOrDefaultAsync(x => x.Id == id);
                var admin = new Admin { 
                Id = findAdmin.Id,
                Name = findAdmin.Name,
                Surname = findAdmin.Surname,
                Email = findAdmin.Email,
                };
                return admin;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> AdminVerify(Admin admin)
        {
            try
            {
                if (!IsValidEmail(admin.Email))
                {
                    throw new Exception("მეილი არავალიდურია");
                }
                var findUser = await this._context.admin.FirstOrDefaultAsync(x => x.Email == admin.Email);
                if (findUser != null)
                {
                    throw new Exception("მსგავსი მეილით ექაუნთი უკვე არსებობს");

                }
                if (string.IsNullOrEmpty(admin.Name) || admin.Name.Length <3)
                {
                    throw new Exception("გთხოვთ შეავსოთ სახელის ველი");
                }
                if (string.IsNullOrEmpty(admin.Surname) || admin.Surname.Length < 3)
                {
                    throw new Exception("გთხოვთ შეავსოთ გვარის ველი");
                }
                var token =  await this._authHelper.GenerateTokenRegister(admin);
               await this._emailservice.SendEmailOtp(admin.Email, token, admin.Name);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> AdminRegister(PasswordModel password,string token)
        {
            try
            {
                if (password.Password != password.ConfirmPassword)
                {
                    throw new ArgumentException("გაიმეორეთ პაროლი სწორად!");
                }
                var regexPattern = @"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*(),.?""':;{}|<>]).{8,16}$";
                
                if (!Regex.IsMatch(password.Password, regexPattern)) {
                    throw new ArgumentException("პაროლი არავალიდურია");
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(password.Password);
                var verifyToken =  await this._authHelper.VerifyJWTToken(token);
                var obj = new Admin
                {
                    Name = verifyToken.Name,
                    Surname = verifyToken.Surname,
                    Email = verifyToken.Email,
                    Password= passwordHash,
                    Created_At= DateTime.Now,
                    Updated_At= DateTime.Now,
                };
                await _context.admin.AddAsync(obj);

                var result = await _context.SaveChangesAsync();

                return result > 0;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<AdminSignInResponse> AdminSignIn(string email, string password)
        {
            try
            {
                var admin = await this._context.admin.FirstOrDefaultAsync(x => x.Email == email);
                if(admin == null)
                {
                    throw new Exception("მოცემული მეილი რეგისტრირებული არ არის");
                }

                var verified = BCrypt.Net.BCrypt.Verify(password, admin.Password);
                if (!verified)
                {
                    throw new Exception("პაროლი არასწორია");
                }


                var token = await this._authHelper.GenerateAdminToken((int)admin.Id);

                var adminObj = new Admin {
                Id=admin.Id,
                Name=admin.Name,
                Email=email,
                Surname=admin.Surname
                };

                return new AdminSignInResponse
                {
                    admin= adminObj,
                    Token = token
                };

            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

   
}
