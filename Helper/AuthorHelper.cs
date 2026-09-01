using FirstProject.Data;
using FirstProject.Models;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Helper
{
    public class AuthorHelper
    {
        public AuthorHelper()
        {
            
        }

        public static bool IsValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        public async Task<AddAuthor> AuthorValidate(AddAuthor author)
        {
            DateTime today = DateTime.Today;
            try
            {
                if (!IsValidEmail(author.Email))
                {
                    throw new Exception("მეილი არავალიდურია");
                }
               

                if (author.Name.Length < 2)
                {
                    throw new Exception("ავტორის სახელი უნდა იყოს ორ სიმბოლოზე მეტი ან ტოლი");
                }

                if (author.Name.Length > 50)
                {
                    throw new Exception("ავტორის სახელი უნდა იყოს 50 სიმბოლოზე ნაკლები ან ტოლი");
                }


                if (author.Surname.Length < 2)
                {
                    throw new Exception("ავტორის გვარი უნდა იყოს ორ სიმბოლოზე მეტი ან ტოლი");
                }

                if (author.Surname.Length > 50)
                {
                    throw new Exception("ავტორის გვარი უნდა იყოს 50 სიმბოლოზე ნაკლები ან ტოლი");
                }

                if (author.PersonalNumber.Length != 11)
                {
                    throw new Exception("პირადი ნომერი უნდა შეადგენდეს 11 სიმბოლოს!");
                }

                int age = today.Year - author.BirthDate.Year;
                if (author.BirthDate.Month > today.Month || (author.BirthDate.Month == today.Month && author.BirthDate.Day > today.Day))
                {
                    age--;
                }

                if (age < 18)
                {
                    throw new Exception("ავტორი უნდა იყოს სრულწლოვანი");
                }


                if (author.PersonalNumber.Length < 4 || author.PersonalNumber.Length > 50)
                {
                    throw new Exception("ნომერი უნდა იყოს 4 სიმბოლოზე მეტი და 50 ზე ნაკლები");
                }
                return author;

            }
            catch (Exception ex) {
               throw new Exception(ex.Message);
            }
        }
    }
}
