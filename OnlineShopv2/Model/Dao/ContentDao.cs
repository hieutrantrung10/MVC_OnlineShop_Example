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
        public IEnumerable<Content> ListAllPaging(int page, int pageSize)
        {
            return db.Contents.OrderByDescending(x=>x.CreatedDate).ToPagedList(page,pageSize);
        }
        //test thêm mới một tin tức
        public long Insert(Content content)
        {
            db.Contents.Add(content);
            db.SaveChanges();

            return content.ID; 
        }
    }
}
