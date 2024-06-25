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
    public class HospitalInfoServices:IHospitalInfo
    {
        private IUnitOfWork _unitOfWork;
        public HospitalInfoServices(IUnitOfWork unitOfWork ) 
        {
            _unitOfWork = unitOfWork;
        }

        public void DeleteHospitalInfo(int id)
        {
            var model = _unitOfWork.Repository<HospitalInfo>().GetById(id);
            _unitOfWork.Repository<HospitalInfo>().Delete(model);
            _unitOfWork.Save();
        }

        public PagedResult<HospitalInfoViewModel> GetAll(int pageNumber, int pageSize)
        {
            var HospitalInfoViewModel=new HospitalInfoViewModel ();
            int totalCount;
            List<HospitalInfoViewModel> vmList=new List<HospitalInfoViewModel>(); 
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                var modelList=_unitOfWork.Repository<HospitalInfo >().GetAll().Skip(ExcludeRecords).Take(pageSize).ToList();
                totalCount=_unitOfWork.Repository<HospitalInfo>().GetAll().Count();
                vmList = ConvertModelViewModelList(modelList);
            }
            catch(Exception)
            {
                throw;

            }
            var result = new PagedResult<HospitalInfoViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize

            };
            return result;
        }

        public HospitalInfoViewModel GetHospitalById(int HospitalID)
        {
            var model = _unitOfWork.Repository<HospitalInfo>().GetById(HospitalID);
            var vm= new  HospitalInfoViewModel(model);
            return vm;
        }

        public void InsertHospitalInfo(HospitalInfoViewModel hospitalInfo)
        {
            var model = new HospitalInfoViewModel().ConvertViewModel(hospitalInfo);
            _unitOfWork.Repository<HospitalInfo>().Add(model);
            _unitOfWork.Save();
        }

        public void UpdateHospitalInfo(HospitalInfoViewModel hospitalInfo)
        {
            var model = new HospitalInfoViewModel().ConvertViewModel(hospitalInfo);
            var ModelById = _unitOfWork.Repository<HospitalInfo>().GetById(model.Id);
            ModelById.Name = hospitalInfo.Name;
            ModelById.City = hospitalInfo.City;
            ModelById.PinCode = hospitalInfo.PinCode;
            ModelById.Country = hospitalInfo.Country;
            _unitOfWork.Repository<HospitalInfo>().Update(ModelById);
            _unitOfWork.Save();

        }

        private  List<HospitalInfoViewModel> ConvertModelViewModelList(List<HospitalInfo> modelList)
        {
            return modelList.Select(x => new HospitalInfoViewModel (x)).ToList();

        }
    }
}
