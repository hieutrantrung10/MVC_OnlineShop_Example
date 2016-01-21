using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
namespace Model.Dao
{
    public class ProductDao
    {
        OnlineShopDbContext db = null;
        public ProductDao()
        {
            db = new OnlineShopDbContext();
        }
        public Product GetByID(long id)
        {
            return db.Products.Find(id);
        }

        public IEnumerable<Product> ListAllPaging(string searchString,int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Code.Contains(searchString));
            }
            return model.OrderByDescending(x=>x.CreatedDate).ToPagedList(page, pageSize);
        }
        public long Insert(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
            return product.ID;
        }

        public bool Update(Product prd)
        {
            try
            {
                var product = db.Products.Find(prd.ID);
                product.Name = prd.Name;
                product.Code = prd.Code;
                product.MetaTitle = prd.MetaTitle;
                product.Description = prd.Description;
                product.Image = prd.Image;
                product.Price = prd.Price;
                product.PromotionPrice = prd.PromotionPrice;
                product.IncludeVAT = prd.IncludeVAT;
                product.Quality = prd.Quality;
                product.CategoryID = prd.CategoryID;
                product.Detail = prd.Detail;
                product.Warranty = prd.Warranty;
                product.ModifiedBy = prd.ModifiedBy;
                product.ModifiedDate = DateTime.Now;
                product.MetaKeywords = prd.MetaKeywords;
                product.MetaDescriptions = prd.MetaDescriptions;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(long id)
        {
            try
            {
                var product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
