using Business.Abstract;
using DataAccess.Abstarct;
using Entities.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrate
{
    public class BlogManager : IBlogService
    {
        IBlog _blog;

        public BlogManager(IBlog blog)
        {
            _blog = blog;
        }

        public void TAdd(Blog t)
        {
            _blog.Create(t);
        }

        public void TDelete(Blog t)
        {
            _blog.Delete(t);
        }

        public Blog TGetByID(int id)
        {
           return _blog.GetById(id);
        }

        public List<Blog> TGetList()
        {
            return _blog.GetList();
        }

        public void TUpdate(Blog t)
        {
            _blog.Update(t);
        }
    }
}
