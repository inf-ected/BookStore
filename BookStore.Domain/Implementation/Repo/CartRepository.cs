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
        //private IEnumerable<Cart> Carts = ApplicationDbContext.Create().Orders;
        public void SaveCart(Cart cart)
        {
            if (cart != null)
            { 
                throw new NotImplementedException("not empl Cart Save from repo");
            }
        }
    }
}
