using System.Collections.Generic;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IMeasureService
    {
        void Add(Measure measure);
        void Edit(int id, Measure measureRequest);
        void Delete(int id);
        Measure GetMeasure(int id);
        List<Measure> AllMeasureList();
        List<Measure> ActiveMeasureList();
        void AddMeasureDetail(MeasureDetail[] measureDetail);
        void AddMeasureDetail(MeasureDetail measureDetail);
        void DeleteMeasureDetail(int id);
        void DeleteMeasureDetail(int[] id);
        MeasureDetail GetMeasureDetail(int id);
        List<MeasureDetail> AllMeasureDetailList(int measureId);
        List<MeasureDetail> ActiveMeasureDetailList(int measureId);
        void EditMeasureDetail(int id, MeasureDetail measureDetail);
    }
}