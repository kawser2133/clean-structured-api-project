using Project.Core.Entities.Business;
using Project.Core.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Interfaces.IRepositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<PaginatedDataViewModel<Order>> GetPaginatedData(int pageNumber, int pageSize);
        Task<OrderViewModel> GetOrderById(int id);
    }
}
