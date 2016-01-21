using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.Dao
{
    public class ContentDao
    {
        OnlineShopDbContext db = null;
        public ContentDao()
        {
            db = new OnlineShopDbContext();
        }
        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
        }
        //test trả về danh sách các bài viết
        public IEnumerable<Content> ListAllPaging(string searchString,int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.CreatedBy.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        //test thêm mới một tin tức
        public long Insert(Content content)
        {
            db.Contents.Add(content);
            db.SaveChanges();

            return content.ID; 
        }

        public bool Update(Content cont)
        {
            try
            {
                var content = db.Contents.Find(cont.ID);
                content.Name = cont.Name;
                content.MetaTitle = cont.MetaTitle;
                content.Description = cont.Description;
                content.Image = cont.Image;
                content.CategoryID = cont.CategoryID;
                content.Detail = cont.Detail;
                content.Warranty = cont.Warranty;
                content.ModifiedBy = cont.ModifiedBy;
                content.ModifiedDate = DateTime.Now;
                content.MetaKeywords = cont.MetaKeywords;
                content.MetaDescriptions = cont.MetaDescriptions;
                content.Status = cont.Status;
                content.Tags = cont.Tags;
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
                var content = db.Contents.Find(id);
                db.Contents.Remove(content);
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
