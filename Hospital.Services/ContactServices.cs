using Hospital.Models;
using Hospital.Repositories.Interfaces;
using Hospital.Utilities;
using Hospital.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Hospital.Services
{
    public class ContactServices:IContactServices
    {
        private IUnitOfWork _unitOfWork;
        public ContactServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void DeleteContact(int id)
        {
            var model = _unitOfWork.Repository<Contact>().GetById(id);
            _unitOfWork.Repository<Contact>().Delete(model);
            _unitOfWork.Save();

        }
        public PagedResult<ContactViewModel> GetAll(int pageNumber, int pageSized)
        {
            var vm = new RoomViewModel();
            int totalCount;
            List<ContactViewModel> vmList = new List<ContactViewModel>();
            try
            {
                int ExcludeRecords = (pageSized * pageNumber) - pageSized;
                var modelList = _unitOfWork.Repository<Contact>().GetAll(includeProperties: "Hospital").Skip(ExcludeRecords).Take(pageSized).ToList();
                totalCount = _unitOfWork.Repository<Contact>().GetAll().Count();
                vmList = ConvertModelViewModelList(modelList);
            }
            catch (Exception)
            {
                throw;

            }
            var result = new PagedResult<ContactViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSized

            };
            return result;
        }
        public ContactViewModel GetContactById(int ContactId)
        {
            var model = _unitOfWork.Repository<Contact>().GetById(ContactId);
            var vm = new ContactViewModel(model);
            return vm;
        }
        
        public void InsertRoom(ContactViewModel Contact)
        {
            var model = new ContactViewModel().ConvertViewModel(Contact);
            _unitOfWork.Repository<Contact>().Add(model);
            _unitOfWork.Save();
        }
        public void UpdateContact(ContactViewModel Contact)
        {
            var model = new ContactViewModel().ConvertViewModel(Contact);
            var ModelById = _unitOfWork.Repository<Contact>().GetById(model.Id);
            ModelById.Phone = Contact.Phone;
            ModelById.Email = Contact.Email;
            ModelById.HospitalId = Contact.HospitalInfoId;
            

            _unitOfWork.Repository<Contact>().Update(ModelById);
            _unitOfWork.Save();

        }
        private List<ContactViewModel> ConvertModelViewModelList(List<Contact> modelList)
        {
            return modelList.Select(x => new ContactViewModel(x)).ToList();
        }
        public void InsertContact(ContactViewModel Contact)
        {
            var model = new ContactViewModel().ConvertViewModel(Contact );
            _unitOfWork.Repository<Contact>().Add(model);
            _unitOfWork.Save();
        }
    }
}
