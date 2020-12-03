using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class CargoService : ICargoService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public CargoService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Cargo cargo)
        {
            var db = _dbFactory();
            db.Cargos.Add(cargo);
            db.SaveChanges();
        }

        public string GetLogoUploadPath()
        {
            return "/Content/Cargo/";
        }

        public void Edit(int id, Cargo cargoRequest)
        {
            var db = _dbFactory();
            var cargo = db.Cargos.FirstOrDefault(a => a.Id == id);
            cargo.DefaultCargo = cargoRequest.DefaultCargo;
            cargo.IsPayDoor = cargoRequest.IsPayDoor;
            cargo.Logo = cargoRequest.Logo;
            cargo.Name = cargoRequest.Name;
            cargo.Price = cargoRequest.Price;
            cargo.Status = cargoRequest.Status;
            db.SaveChanges();
        }

        public Cargo GetCargo(int id)
        {
            var db = _dbFactory();
            var cargo = db.Cargos.FirstOrDefault(a => a.Id == id);
            return cargo;
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var cargo = db.Cargos.FirstOrDefault(a => a.Id == id);
            db.Cargos.Remove(cargo);
            db.SaveChanges();
        }

        public List<Cargo> AllCargoList()
        {
            var db = _dbFactory();
            var list = db.Cargos.OrderBy(a => a.Price).ToList();
            return list;
        }

        public List<Cargo> ActiveCargoList()
        {
            var db = _dbFactory();
            var list = db.Cargos.Where(a => a.Status == AdminProject.Models.StatusTypes.Active && !a.IsPayDoor).OrderBy(a => a.Price).ToList();
            return list;
        }

        public List<Cargo> ActiveDoorPayList()
        {
            var db = _dbFactory();
            var list = db.Cargos.Where(a => a.Status == AdminProject.Models.StatusTypes.Active && a.IsPayDoor).OrderBy(a => a.Price).ToList();
            return list;
        }

        public Cargo GetDefaultCargo()
        {
            var db = _dbFactory();
            var cargo = db.Cargos.OrderBy(a => a.Price).FirstOrDefault(a => a.Status == AdminProject.Models.StatusTypes.Active && a.DefaultCargo);

            if (cargo == null) //kargo firmasi yoksa default olarak belirli bir fiyat ver.
                cargo = new Cargo
                {
                    Price = 7,
                    IsPayDoor = false,
                    Name = "Sabit kargo ücreti",
                    Status = AdminProject.Models.StatusTypes.Active,
                    Id = 0,
                    DefaultCargo = true,
                    Logo = ""
                };
            return cargo;
        }
    }
}