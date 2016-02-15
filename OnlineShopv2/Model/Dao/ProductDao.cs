﻿using Model.EF;
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
        /// <summary>
        /// Lấy ra danh mục các sản phẩm trên cùng danh mục
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<Product> ListByCategoryId(long categoryId, ref int totalRecord, int pageIndex = 1, int pageSize = 10)
        {
            totalRecord = db.Products.Where(x => x.CategoryID == categoryId).Count();
            var model = db.Products.Where(x => x.CategoryID == categoryId).OrderByDescending(x=>x.CreatedDate).Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();
            return model;
        }
        /// <summary>
        /// Lấy ra danh sách các sản phẩm mới
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x=>x.CreatedDate).Take(top).ToList();
        }
        /// <summary>
        /// Lấy ra các sản phẩm bán chạy
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        /// <summary>
        /// Lấy ra các sản phẩm liên quan
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<Product> ListRelatedProducts(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).ToList();
        }
        
    }
}
