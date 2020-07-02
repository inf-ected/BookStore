using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Implementation.Repo
{
    public class CartRepository : ICartRepository
    {
        private ApplicationDbContext context =  new ApplicationDbContext();
        public void SaveCart(Cart cart)
        {
            if (cart != null)
            {
                if (cart.Id == 0)
                   context.Carts.Add(cart);
                context.SaveChanges();
                //throw new NotImplementedException("not empl Cart Save from repo");
            }
        }


        //public IEnumerable<CartLine> Lines
        //{
        //    get
        //    {
        //        return Carts.;
        //    }
        //}

        public void AddItem(Cart cart, Product product, int quantity)
        {
            var _linesCollection = context.Carts.FirstOrDefault(x => x.Id == cart.Id).LinesCollection;

            var line = _linesCollection.Where(p => p.Product.Id == product.Id).FirstOrDefault();
            if (line == null)
            {
                _linesCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void RemoveItem(Cart cart, Product product)
        {
            var _linesCollection = context.Carts.Where(x => x.Id == cart.Id).FirstOrDefault().LinesCollection;

            _linesCollection.RemoveAll(x => x.Product.Id == product.Id);
        }

        public decimal ComputeTotalValue(Cart cart)
        {
            var _linesCollection = context.Carts.Where(x => x.Id == cart.Id).FirstOrDefault().LinesCollection;

            return _linesCollection.Sum(x => x.Product.Price * x.Quantity);
        }

        public void Clear(Cart cart)
        {
            var _linesCollection = context.Carts.Where(x => x.Id == cart.Id).FirstOrDefault().LinesCollection;

            _linesCollection.Clear();
        }


    }
}
