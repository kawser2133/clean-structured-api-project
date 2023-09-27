using Microsoft.EntityFrameworkCore;
using Project.Core.Entities.Business;
using Project.Core.Entities.General;
using Project.Core.Exceptions;
using Project.Core.Interfaces.IRepositories;
using Project.Infrastructure.Data;

namespace Project.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<PaginatedDataViewModel<Order>> GetPaginatedData(int pageNumber, int pageSize)
        {
            var query = _dbContext.Orders
                .Include(c => c.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking();

            var data = await query.ToListAsync();
            var totalCount = await _dbContext.Orders.CountAsync();

            return new PaginatedDataViewModel<Order>(data, totalCount);
        }

        public async Task<OrderViewModel> GetOrderById(int id)
        {
            // If want to use LINQ Query Syntax
            //var query = from order in _dbContext.Orders
            //            where order.Id == id
            //            join customer in _dbContext.Customers on order.CustomerId equals customer.Id
            //            join orderDetails in _dbContext.OrderDetails on order.Id equals orderDetails.OrderId
            //            join product in _dbContext.Products on orderDetails.ProductId equals product.Id
            //            select new { Order = order, OrderDetails = orderDetails, Product = product, Customer = customer };

            //var result = await query.AsNoTracking().ToListAsync();
            //var data = result.First().Order;
            //var orderDetailsData = result;

            //If want to use LINQ Method Syntax
            var data = await _dbContext.Orders.Where(x => x.Id == id)
                .Include(c => c.Customer)
                .Include(x => x.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync();

            if (data == null)
            {
                throw new NotFoundException("No data found");
            }

            var orderData = new OrderViewModel
            {
                Id = data.Id,
                CustomerId = data.CustomerId,
                CustomerName = data?.Customer?.FullName,
                TotalBill = data.TotalBill,
                TotalQuantity = data.TotalQuantity,
                Description = data.Description,
                ProcessingData = data.ProcessingData,
                OrderDetails = data.OrderDetails.Select(orderDetail => new OrderDetailsViewModel
                {
                    OrderId = orderDetail.Id,
                    ProductId = orderDetail.ProductId,
                    ProductName = orderDetail?.Product?.Name,
                    SellingPrice = orderDetail.SellingPrice,
                    Quantity = orderDetail.Quantity,
                    Description = orderDetail.Description
                }).ToList()
            };

            return orderData;
        }
    }
}
