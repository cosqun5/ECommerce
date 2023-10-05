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
    public class ProductManager : IProductService
    {
        IProduct _product;
        public ProductManager(IProduct product)
        {
            _product = product;
        }
        public void TAdd(Product t)
        {
            _product.Create(t);
        }

        public void TDelete(Product t)
        {
           _product.Delete(t);
        }

        public Product TGetByID(int id)
        {
           return _product.GetById(id);
        }

        public List<Product> TGetList()
        {
            return _product.GetList();
        }

        public void TUpdate(Product t)
        {
            _product.Update(t);
        }
    }
}
