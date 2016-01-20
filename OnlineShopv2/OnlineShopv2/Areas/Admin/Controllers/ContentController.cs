using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace OnlineShopv2.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        // GET: Admin/Content
        public ActionResult Index(int page = 1, int pageSize=10)
        {
            var dao = new ContentDao();
            var model = dao.ListAllPaging(page, pageSize);
            return View(model as IPagedList<Content>);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Content model)
        {
            if (ModelState.IsValid)
            {
                //test thêm mới nội dung tin tức 29/12/2015
                var dao = new ContentDao();
                long id = dao.Insert(model);
                if (id > 0)
                {
                    SetAlert("Thêm mới tin tức thành công!","success");
                    return RedirectToAction("Create","Content");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm tin tức không thành công!!");
                }

            }
            SetViewBag();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new ContentDao();
            var content = dao.GetByID(id);
            SetViewBag(content.CategoryID);
            return View(content);
        }
       
        [HttpPost]
        public ActionResult Edit(Content content)
        {
            if (ModelState.IsValid)
            {
                var dao = new ContentDao();
                var result = dao.Update(content);
                if (result)
                {
                    return RedirectToAction("Index", "Content");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật tin tức thất bại !!!");
                }
            }
            SetViewBag(content.CategoryID);
            return View();
        }
        public void SetViewBag(long ? selectedId = null)
        {
            var dao = new CategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(),"ID","Name",selectedId);
        }
    }
}