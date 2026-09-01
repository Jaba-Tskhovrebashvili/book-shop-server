
using FirstProject.Helper;
using FirstProject.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace FirstProject.Data
{
    public class PRODUCT_PKG
    {
        private readonly AddDbContext _context;
        private readonly ProductHelper _productHelper;
        public PRODUCT_PKG(AddDbContext context, ProductHelper productHelper)
        {
            this._context= context;
            this._productHelper= productHelper;
        }
        
        public async Task<bool> AddProductFunct(AddProduct addProduct)
        {
            try
            {
                var productValidate= await this._productHelper.ProductValidate(addProduct);
                var createProduct = new Product {
                     Name= productValidate.Name,
                    Annotation= productValidate.Annotation,
                    typeId= productValidate.typeId,
                    ISBN= productValidate.ISBN,
                    release_date= productValidate.release_date,
                    publishId= productValidate.publishId,
                    page_quantity= productValidate.page_quantity,
                    address= productValidate.address,
                    Created_At= DateTime.Now,
                    Updated_At= DateTime.Now
                };
               
                foreach (var author in productValidate.Authors)
                {
                    var findAuthor = await this._context.author.FirstOrDefaultAsync(x => x.Id == author.id);
                    if (findAuthor == null) {
                        throw new Exception("დაფიქსირდა შეცდომა");
                    }
                    createProduct.Authors.Add(findAuthor);
                }

                await this._context.product.AddAsync(createProduct);
                await this._context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> EditProductFunct(AddProduct addProduct, int productId)
        {
            try
            {
                var productValidate = await this._productHelper.ProductValidate(addProduct);

                var findProduct=await this._context.product.Include(p=>p.Authors).FirstOrDefaultAsync(x => x.Id == productId);
                if (findProduct == null) {
                    throw new Exception("დაფიქსირდა შეცდომა");
                }

                findProduct.Name = productValidate.Name;
                findProduct.Annotation = productValidate.Annotation;
                findProduct.typeId = productValidate.typeId;
                findProduct.ISBN = productValidate.ISBN;
                findProduct.release_date = productValidate.release_date;
                findProduct.publishId = productValidate.publishId;
                findProduct.page_quantity = productValidate.page_quantity;
                findProduct.address = productValidate.address;
                findProduct.Updated_At = DateTime.Now;
                findProduct.Authors = new List<Author>();

                foreach (var author in productValidate.Authors)
                {
                    var findAuthor = await this._context.author.FirstOrDefaultAsync(x => x.Id == author.id);
                    if (findAuthor == null)
                    {
                        throw new Exception("დაფიქსირდა შეცდომა");
                    }
                    findProduct.Authors.Add(findAuthor);
                }

                await this._context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteProductFunct(int productId)
        {
            try
            {
                var findProduct = await this._context.product.FirstOrDefaultAsync(x=>x.Id==productId);
                if (findProduct == null)
                {
                    throw new Exception("დაფიქსირდა შეცდომა");
                }

                this._context.product.Remove(findProduct);
                await this._context.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
