using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using PagedList;

namespace OnlineShopv2.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index(string searchString,int page = 1, int pageSize=10)
        {
            var dao = new ProductDao();
            var product = dao.ListAllPaging(searchString,page, pageSize);
            ViewBag.SearchString = searchString;
            return View(product as IPagedList<Product>);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                long id = dao.Insert(product);
                if (id > 0)
                {
                    SetAlert("Thêm mới sản phẩm thành công!", "success");
                    return RedirectToAction("Create", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm sản phẩm không thành công!!");
                }
            }
            SetViewBag();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new ProductDao();
            var product = dao.GetByID(id);
            SetViewBag(product.CategoryID);

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                var result = dao.Update(product);
                if (result)
                {
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("","Cập nhật sản phẩm thất bại !!!");
                }
            }
            SetViewBag(product.CategoryID);
            return View();
        }
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            var dao = new ProductDao();
            dao.Delete(id);
            return RedirectToAction("Index");
        }
        public void SetViewBag(long ? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(),"ID","Name",selectedId);
        }

       
    }
}