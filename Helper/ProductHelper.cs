using FirstProject.Models;

namespace FirstProject.Helper
{
    public class ProductHelper
    {
        public ProductHelper() { }

        public async Task<AddProduct> ProductValidate(AddProduct addProduct)
        {
            try
            {
                if (addProduct.Name.Length < 2 || addProduct.Name.Length>250)
                {
                    throw new Exception("პროდუქტის სახელი უნდა იყოს მინიმუმ 2 და მაქსიმუმ 250 სიმბოლო");
                }

                if (addProduct.Annotation.Length < 100 || addProduct.Annotation.Length > 500)
                {
                    throw new Exception("პროდუქტის ანოტაცია უნდა იყოს მინიმუმ 100 და მაქსიმუმ 500 სიმბოლო");
                }

                if (addProduct.ISBN.Length !=13)
                {
                    throw new Exception("პროდუქტის ISBN უნდა შეადგენდეს 13 სიმბოლოს");
                }

                if (addProduct.page_quantity <= 0)
                {
                    throw new Exception("პროდუქტი უნდა იყოს მინიმუმ 1 გვერდიანი");
                }
                if (string.IsNullOrEmpty(addProduct.address))
                {
                    throw new Exception("გთხოვთ მიუთითეთ მისამართი");
                }

                if (addProduct.Authors.Count <= 0)
                {
                    throw new Exception("მიუთითეთ მინიმუმ 1 ავტორი");
                }

                if (addProduct.typeId == 0 || addProduct.typeId==null)
                {
                    throw new Exception("მიუთითეთ პროდუქტს ტიპი");
                }
                if (addProduct.publishId == 0 || addProduct.publishId == null)
                {
                    throw new Exception("მიუთითეთ პროდუქტს გამომცემლობა");
                }

                return addProduct;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
