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
    public class CustomerService : ICustomerService
    {
        private readonly IBaseMapper<Customer, CustomerViewModel> _customerViewModelMapper;
        private readonly IBaseMapper<CustomerViewModel, Customer> _customerMapper;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(
            IBaseMapper<Customer, CustomerViewModel> customerViewModelMapper,
            IBaseMapper<CustomerViewModel, Customer> customerMapper,
            ICustomerRepository customerRepository)
        {
            _customerMapper = customerMapper;
            _customerViewModelMapper = customerViewModelMapper;
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerViewModel>> GetCustomers()
        {
            return _customerViewModelMapper.MapList(await _customerRepository.GetAll());
        }

        public async Task<PaginatedDataViewModel<CustomerViewModel>> GetPaginatedCustomers(int pageNumber, int pageSize)
        {
            //Get peginated data
            var paginatedData = await _customerRepository.GetPaginatedData(pageNumber, pageSize);

            //Map data with ViewModel
            var mappedData = _customerViewModelMapper.MapList(paginatedData.Data);

            var paginatedDataViewModel = new PaginatedDataViewModel<CustomerViewModel>(mappedData.ToList(), paginatedData.TotalCount);

            return paginatedDataViewModel;
        }

        public async Task<CustomerViewModel> GetCustomer(int id)
        {
            return _customerViewModelMapper.MapModel(await _customerRepository.GetById(id));
        }

        public async Task<bool> IsExists(string key, string value)
        {
            return await _customerRepository.IsExists(key, value);
        }

        public async Task<bool> IsExistsForUpdate(int id, string key, string value)
        {
            return await _customerRepository.IsExistsForUpdate(id, key, value);
        }

        public async Task<CustomerViewModel> Create(CustomerViewModel model)
        {
            //Mapping through AutoMapper
            var entity = _customerMapper.MapModel(model);
            entity.EntryDate = DateTime.Now;

            return _customerViewModelMapper.MapModel(await _customerRepository.Create(entity));
        }

        public async Task Update(CustomerViewModel model)
        {
            var existingData = await _customerRepository.GetById(model.Id);

            //Manual mapping
            existingData.FullName = model.FullName;
            existingData.Email = model.Email;
            existingData.UpdateDate = DateTime.Now;

            await _customerRepository.Update(existingData);
        }

        public async Task Delete(int id)
        {
            var entity = await _customerRepository.GetById(id);
            await _customerRepository.Delete(entity);
        }

    }
}
