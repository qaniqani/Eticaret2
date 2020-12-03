using System.Collections.Generic;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;

namespace AdminProject.Services.Interface
{
    public interface IAddressService
    {
        void Freeze(int id);
        void Add(Address address);
        void Edit(int id, Address addressRequest);
        Address GetAddress(int id);
        Address GetAddress(int id, int userId);
        List<Address> GetUserAddressAllList(int userId);
        List<Address> GetUserAddressActiveList(int userId);
        List<Address> GetUserAddressUserActiveList(int userId);
        List<Address> GetUserAddressActiveList(int userId, StatusTypes status);
        void Delete(int id);
    }
}