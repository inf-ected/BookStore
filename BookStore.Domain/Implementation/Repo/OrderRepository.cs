using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Implementation.Repo
{
   public class OrderRepository : IOrderRepository
    {
        private ApplicationDbContext _context =  new ApplicationDbContext();
        public IEnumerable<Order> Orders
        {
            get
            {
                return _context.Orders;
            }
        }


        public int GetLastOrder()
        {
            //var id = Orders.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            if (Orders.Count()==0)
                return 1;
            

            return Orders.Max(x => x.Id) + 1;
        }

        public void SaveOrder(Order order)
        {
            if (order != null)
            {
                //VALIDATION
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
        }
        public void SaveCart(Cart cart)
        {
            if (cart != null)
            {
                // ну и как тут достучаться до Cart ?
                throw new NotImplementedException("not empl Cart Save");
            }
        }
    }
}
