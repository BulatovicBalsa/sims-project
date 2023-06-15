using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Core.TimeOffRequests.Models;
using Hospital.Core.Workers.Models;
using Hospital.Serialization;

namespace Hospital.Core.TimeOffRequests.Repositories;

public class DoctorTimeOffRequestRepository
{
    private const string FilePath = "../../../Data/doctorTimeOffRequests.csv";
    private readonly ISerializer<DoctorTimeOffRequest> _serializer;

    public DoctorTimeOffRequestRepository(ISerializer<DoctorTimeOffRequest> serializer)
    {
        _serializer = serializer;
    }

    public List<DoctorTimeOffRequest> GetAll()
    {
        return _serializer.Load(FilePath);
    }

    public DoctorTimeOffRequest? GetById(string id)
    {
        return GetAll().Find(request => request.Id == id);
    }

    public void Add(DoctorTimeOffRequest request)
    {
        var allRequests = GetAll();

        allRequests.Add(request);

        _serializer.Save(allRequests, FilePath);
    }

    public void Update(DoctorTimeOffRequest request)
    {
        var allRequests = GetAll();

        var indexToUpdate = allRequests.FindIndex(e => e.Id == request.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allRequests[indexToUpdate] = request;

        _serializer.Save(allRequests, FilePath);
    }

    public void Delete(DoctorTimeOffRequest request)
    {
        var allRequests = GetAll();

        var indexToDelete = allRequests.FindIndex(e => e.Id == request.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allRequests.RemoveAt(indexToDelete);

        _serializer.Save(allRequests, FilePath);
    }

    public void DeleteAll()
    {
        var emptyDoctorTimeOffRequestList = new List<DoctorTimeOffRequest>();
        _serializer.Save(emptyDoctorTimeOffRequestList, FilePath);
    }

    public List<DoctorTimeOffRequest> GetDoctorTimeOffRequests(Doctor doctor)
    {
        return GetAll().Where(request => request.DoctorId == doctor.Id).ToList();
    }

    public List<DoctorTimeOffRequest> GetAllNonExpiredDoctorTimeOffRequests()
    {
        return GetAll().Where(request => request.Start > DateTime.Today).ToList();
    }

    public List<DoctorTimeOffRequest> GetNonExpiredDoctorTimeOffRequests(Doctor doctor)
    {
        return GetAll().Where(request => request.Start > DateTime.Today && request.DoctorId == doctor.Id).ToList();
    }
}