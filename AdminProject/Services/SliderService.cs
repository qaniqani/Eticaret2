using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class SliderService : ISliderService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public SliderService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public List<Slider> List()
        {
            var db = _dbFactory();
            var sliders = db.Sliders.OrderBy(a => a.SequenceNumber).Where(a => a.Status == StatusTypes.Active && a.PictureType == PictureTypes.Slider).ToList();

            return sliders;
        }
    }
}