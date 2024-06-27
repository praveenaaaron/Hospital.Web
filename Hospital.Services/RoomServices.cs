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
    public class RoomServices : IRoomServices
    {
        private IUnitOfWork _unitOfWork;
        public RoomServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void DeleteRoom(int id)
        {
            var model=_unitOfWork.Repository<Room>().GetById (id);  
            _unitOfWork.Repository<Room>().Delete(model);
            _unitOfWork.Save();

        }

        public PagedResult<RoomViewModel> GetAll(int pageNumber, int pageSized)
        {
            var vm= new RoomViewModel();
            int totalCount;
            List<RoomViewModel> vmList = new List<RoomViewModel>();
            try
            {
                int ExcludeRecords = (pageSized * pageNumber) - pageSized;
                var modelList = _unitOfWork.Repository<Room>().GetAll().Skip(ExcludeRecords).Take(pageSized).ToList();
                totalCount = _unitOfWork.Repository<Room>().GetAll().Count();
                vmList = ConvertModelViewModelList(modelList);
            }
            catch (Exception)
            {
                throw;

            }
            var result = new PagedResult<RoomViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSized

            };
            return result;
        }

        private List<RoomViewModel> ConvertModelViewModelList(List<Room> modelList)
        {
            return modelList.Select(x => new RoomViewModel(x)).ToList();
        }

        

        public RoomViewModel GetRoomById(int RoomId)
        {
            throw new NotImplementedException();
        }
        public RoomViewModel HospitalById(int HospitalId)
        {
            var model=_unitOfWork.Repository<Room>().GetById (HospitalId);
            var vm = new RoomViewModel(model);
            return vm;
        }
        public void InsertRoom(RoomViewModel Room)
        {
            var model = new RoomViewModel().ConvertViewModel(Room);
            _unitOfWork.Repository<Room>().Add(model);
            _unitOfWork.Save();
        }
        public void UpdateRoom(RoomViewModel Room)
        {
            var model = new RoomViewModel().ConvertViewModel(Room);
            var ModelById = _unitOfWork.Repository<Room>().GetById(model.Id);
            ModelById.Type = Room.Type;
            ModelById.RoomNumber = Room.RoomNumber;
            ModelById.Status  = Room.Status;
            
            _unitOfWork.Repository<Room>().Update(ModelById);
            _unitOfWork.Save();

        }
       

    }
}
