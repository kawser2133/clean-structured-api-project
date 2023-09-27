using Project.Core.Entities.Business;
using Project.Core.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Interfaces.IServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderViewModel>> GetOrders();
        Task<PaginatedDataViewModel<OrderViewModel>> GetPaginatedOrders(int pageNumber, int pageSize);
        Task<OrderViewModel> GetOrder(int id);
        Task<bool> IsExists(string key, string value);
        Task<bool> IsExistsForUpdate(int id, string key, string value);
        Task<OrderViewModel> Create(OrderViewModel model);
        Task Update(OrderViewModel model);
        Task Delete(int id);
    }
}
