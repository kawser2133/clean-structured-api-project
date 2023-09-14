using Microsoft.EntityFrameworkCore;
using Project.Core.Entities.Business;
using Project.Core.Entities.General;
using Project.Core.Interfaces.IRepositories;
using Project.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<OrderViewModel> GetOrderById(int id)
        {
            var orderData = await (from order in _dbContext.Orders
                                   where order.Id == id
                                   select order).FirstOrDefaultAsync();

            var orderDetailsData = await (from orderDetails in _dbContext.OrderDetails
                                          where orderDetails.OrderId == id
                                          select orderDetails).ToListAsync();


            var orderDetailsViewModel = new List<OrderDetailsViewModel>();
            foreach (var order in orderDetailsData)
            {
                var detailsData = new OrderDetailsViewModel
                {
                    OrderId = order.OrderId,
                    ProductId = order.ProductId,
                    Quantity = order.Quantity,
                    SellingPrice = order.SellingPrice,
                    Description = order.Description,
                };
                orderDetailsViewModel.Add(detailsData);
            }

            var orderViewModel = new OrderViewModel
            {
                CustomerId = orderData.CustomerId,
                TotalBill = orderData.TotalBill,
                TotalQuantity = orderData.TotalQuantity,
                ProcessingData = orderData.ProcessingData,
                Description = orderData.Description,

                OrderDetails = orderDetailsViewModel
            };

            return orderViewModel;

        }
    }
}
