
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

        public async Task<object> AuthorProducts(int authorId, int page)
        {
            int pageSize = 5;
            try
            {
                var products = from product in this._context.product
                               join AuthProd in this._context.Set<Dictionary<string, object>>("AuthorProducts")
                               on product.Id equals AuthProd["ProductId"]
                               where (int)AuthProd["AuthorId"] == authorId
                               select new Product
                               {
                                   Id = product.Id,
                                   Name = product.Name,
                                   Annotation = product.Annotation,
                                   typeId = product.typeId,
                                   Product_Type = product.Product_Type,
                                   ISBN = product.ISBN,
                                   release_date = product.release_date,
                                   publishId = product.publishId,
                                   publishing_house = product.publishing_house,
                                   page_quantity = product.page_quantity,
                                   address = product.address,
                                   Created_At = product.Created_At,
                                   Updated_At = product.Updated_At
                               };

                var count = await products.CountAsync();

                var result = await products
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalCount = (int)Math.Ceiling((double)count / pageSize);

                return new { products= result, totalCount};
                
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<object> GetProducts(int? typeId, int? publishId, string? search, int page)
        {
            int pageSize = 5;
            try
            {
                var query =  this._context.product
                               .Where(x => (string.IsNullOrEmpty(search) || EF.Functions.Like((x.Name).ToLower(), $"%{search.ToLower()}%")) &&
                               (typeId == null || x.typeId == typeId) && (publishId == null || x.publishId == publishId));
                               

                var count = await query.CountAsync();

                var products = await query
                    .OrderByDescending(x=>x.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(product => new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Annotation = product.Annotation,
                        typeId = product.typeId,
                        Product_Type = product.Product_Type,
                        ISBN = product.ISBN,
                        release_date = product.release_date,
                        publishId = product.publishId,
                        publishing_house = product.publishing_house,
                        page_quantity = product.page_quantity,
                        address = product.address,
                        Created_At = product.Created_At,
                        Updated_At = product.Updated_At,
                        Authors = product.Authors.Select(x => new Author
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Surname = x.Surname
                        }).ToList()

                    })
                    .ToListAsync();

                var totalCount = (int)Math.Ceiling((double)count / pageSize);

                return new { products, totalCount };


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddProductFunct(AddProduct addProduct)
        {
            try
            {

                var checkISBN = await this._context.product.FirstOrDefaultAsync(x => x.ISBN == addProduct.ISBN);
                if (checkISBN != null) {
                    throw new Exception("მსგავსი ISBN ით პროდუქტი უკვე დამატებულია");
                }

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



        public async Task<object> GetProducttypes(string? search)
        {

            try
            {

                var query = await this._context.product_types
                           .Where(x => (string.IsNullOrEmpty(search) || EF.Functions.Like(x.name, $"{search.ToLower()}%")))
                            .Select(a => new Product_Type
                            {
                               Id = a.Id,
                                name = a.name
                            })
                            .ToListAsync();
                    return new {product_types= query };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<object> GetPublishingHouse(string? search, int page)
        {
            int pageSize = 15;


            try
            {

                var query = this._context.publishing_houses
             .Where(x => (string.IsNullOrEmpty(search) || EF.Functions.Like(x.name, $"{search.ToLower()}%")));

                var count = await query.CountAsync();


                var publishing_houses = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(a => new Publishing_house
                    {
                        Id = a.Id,
                        name = a.name
                    })
                    .ToListAsync();

                int totalPages = (int)Math.Ceiling((double)count / pageSize);

                return new { publishing_houses, totalPages };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
