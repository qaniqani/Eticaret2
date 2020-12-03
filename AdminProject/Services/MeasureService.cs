using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class MeasureService : IMeasureService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public MeasureService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Measure measure)
        {
            var db = _dbFactory();
            db.Measures.Add(measure);
            db.SaveChanges();
        }

        public void Edit(int id, Measure measureRequest)
        {
            var db = _dbFactory();
            var measure = db.Measures.FirstOrDefault(a => a.Id == id);
            measure.Name = measureRequest.Name;
            measure.Status = measureRequest.Status;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var measure = db.Measures.FirstOrDefault(a => a.Id == id);
            db.Measures.Remove(measure);
            db.SaveChanges();
        }

        public Measure GetMeasure(int id)
        {
            var db = _dbFactory();
            var measure = db.Measures.FirstOrDefault(a => a.Id == id);
            return measure;
        }

        public List<Measure> AllMeasureList()
        {
            var db = _dbFactory();
            var measures = db.Measures.OrderBy(a => a.Name).ToList();
            return measures;
        }

        public List<Measure> ActiveMeasureList()
        {
            var db = _dbFactory();
            var measures =
                db.Measures.Where(a => a.Status == AdminProject.Models.StatusTypes.Active).OrderBy(a => a.Name).ToList();
            return measures;
        }

        public void AddMeasureDetail(MeasureDetail[] measureDetail)
        {
            var db = _dbFactory();
            db.MeasureDetails.AddRange(measureDetail);
            db.SaveChanges();
        }

        public void AddMeasureDetail(MeasureDetail measureDetail)
        {
            var db = _dbFactory();
            db.MeasureDetails.Add(measureDetail);
            db.SaveChanges();
        }

        public void EditMeasureDetail(int id, MeasureDetail measureDetail)
        {
            var db = _dbFactory();
            var measure = db.MeasureDetails.FirstOrDefault(a => a.Id == id);
            if (measure == null)
                return;

            measure.Size = measureDetail.Size;
            measure.Status = measureDetail.Status;
            db.SaveChanges();
        }

        public void DeleteMeasureDetail(int id)
        {
            var db = _dbFactory();
            var measure = db.MeasureDetails.FirstOrDefault(a => a.Id == id);
            db.MeasureDetails.Remove(measure);
            db.SaveChanges();
        }

        public void DeleteMeasureDetail(int[] id)
        {
            var db = _dbFactory();
            var measures = db.MeasureDetails.Where(a => id.Contains(a.Id));
            db.MeasureDetails.RemoveRange(measures);
            db.SaveChanges();
        }

        public MeasureDetail GetMeasureDetail(int id)
        {
            var db = _dbFactory();
            var measureDetail = db.MeasureDetails.FirstOrDefault(a => a.Id == id);
            return measureDetail;
        }

        public List<MeasureDetail> AllMeasureDetailList(int measureId)
        {
            var db = _dbFactory();
            var measureList = db.MeasureDetails.Where(a => a.MeasureId == measureId).OrderBy(a => a.Size).ToList();
            return measureList;
        }

        public List<MeasureDetail> ActiveMeasureDetailList(int measureId)
        {
            var db = _dbFactory();
            var measureList = db.MeasureDetails.Where(a => a.MeasureId == measureId && a.Status == AdminProject.Models.StatusTypes.Active).OrderBy(a => a.Size).ToList();
            return measureList;
        }
    }
}