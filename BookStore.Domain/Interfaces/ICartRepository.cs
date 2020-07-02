using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Interfaces
{
 public   interface ICartRepository
    {
        void SaveCart(Cart cart);

      //  IEnumerable<CartLine> Lines{get;}
        void AddItem(Cart cart, Product product, int quantity);
        void RemoveItem(Cart cart, Product product);
        decimal ComputeTotalValue(Cart cart);
        void Clear(Cart cart);
    }
}
