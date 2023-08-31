using Project.Core.Entities.Business;
using Project.Core.Entities.General;
using Project.Core.Exceptions;
using Project.Core.Interfaces.IMapper;
using Project.Core.Interfaces.IRepositories;
using Project.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseMapper<Order, OrderViewModel> _orderViewModelMapper;
        private readonly IBaseMapper<OrderViewModel, Order> _orderMapper;
        private readonly IOrderRepository _orderRepository;

        public OrderService(
            IBaseMapper<Order, OrderViewModel> orderViewModelMapper,
            IBaseMapper<OrderViewModel, Order> orderMapper,
            IOrderRepository orderRepository)
        {
            _orderMapper = orderMapper;
            _orderViewModelMapper = orderViewModelMapper;
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrders()
        {
            return _orderViewModelMapper.MapList(await _orderRepository.GetAll());
        }

        public async Task<PaginatedDataViewModel<OrderViewModel>> GetPaginatedOrders(int pageNumber, int pageSize)
        {
            //Get peginated data
            var paginatedData = await _orderRepository.GetPaginatedData(pageNumber, pageSize);

            //Map data with ViewModel
            var mappedData = _orderViewModelMapper.MapList(paginatedData.Data);

            var paginatedDataViewModel = new PaginatedDataViewModel<OrderViewModel>(mappedData.ToList(), paginatedData.TotalCount);

            return paginatedDataViewModel;
        }

        public async Task<OrderViewModel> GetOrder(int id)
        {
            return _orderViewModelMapper.MapModel(await _orderRepository.GetById(id));
        }

        public async Task<bool> IsExists(string key, string value)
        {
            return await _orderRepository.IsExists(key, value);
        }

        public async Task<bool> IsExistsForUpdate(int id, string key, string value)
        {
            return await _orderRepository.IsExistsForUpdate(id, key, value);
        }

        public async Task<OrderViewModel> Create(OrderViewModel model)
        {
            //Mapping through AutoMapper
            var entity = _orderMapper.MapModel(model);
            entity.EntryDate = DateTime.Now;

            return _orderViewModelMapper.MapModel(await _orderRepository.Create(entity));
        }

        public async Task Update(OrderViewModel model)
        {
            var existingData = await _orderRepository.GetById(model.Id);

            //Manual mapping
            //existingData.FullName = model.FullName;
            //existingData.Email = model.Email;
            existingData.UpdateDate = DateTime.Now;

            await _orderRepository.Update(existingData);
        }

        public async Task Delete(int id)
        {
            var entity = await _orderRepository.GetById(id);
            await _orderRepository.Delete(entity);
        }

    }
}
