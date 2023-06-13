using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Injectors;
using Hospital.Models.Requests;
using Hospital.Serialization;

namespace Hospital.Repositories.Requests;
using Hospital.Models.Doctor;

public class DoctorTimeOffRequestRepository
{
    private const string FilePath = "../../../Data/doctorTimeOffRequests.csv";
    private static DoctorTimeOffRequestRepository? _instance;
    private static readonly ISerializer<DoctorTimeOffRequest> Serializer = SerializerInjector.CreateInstance<ISerializer<DoctorTimeOffRequest>>();

    public static DoctorTimeOffRequestRepository Instance => _instance ??= new DoctorTimeOffRequestRepository();

    public List<DoctorTimeOffRequest> GetAll()
    {
        return Serializer.Load(FilePath);
    }

    public DoctorTimeOffRequest? GetById(string id)
    {
        return GetAll().Find(request => request.Id == id);
    }

    public void Add(DoctorTimeOffRequest request)
    {
        var allRequests = GetAll();

        allRequests.Add(request);

        Serializer.Save(allRequests, FilePath);
    }

    public void Update(DoctorTimeOffRequest request)
    {
        var allRequests = GetAll();

        var indexToUpdate = allRequests.FindIndex(e => e.Id == request.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allRequests[indexToUpdate] = request;

        Serializer.Save(allRequests, FilePath);
    }

    public void Delete(DoctorTimeOffRequest request)
    {
        var allRequests = GetAll();

        var indexToDelete = allRequests.FindIndex(e => e.Id == request.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allRequests.RemoveAt(indexToDelete);

        Serializer.Save(allRequests, FilePath);
    }

    public static void DeleteAll()
    {
        var emptyDoctorTimeOffRequestList = new List<DoctorTimeOffRequest>();
        Serializer.Save(emptyDoctorTimeOffRequestList, FilePath);
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