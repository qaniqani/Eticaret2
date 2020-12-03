using System.Collections.Generic;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface ICargoService
    {
        void Add(Cargo cargo);
        void Edit(int id, Cargo cargoRequest);
        Cargo GetCargo(int id);
        void Delete(int id);
        List<Cargo> AllCargoList();
        List<Cargo> ActiveCargoList();
        List<Cargo> ActiveDoorPayList();
        Cargo GetDefaultCargo();
        string GetLogoUploadPath();
    }
}