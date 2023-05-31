using System.Collections.Generic;
using Hospital.Models.Requests;
using Hospital.Serialization;

namespace Hospital.Repositories.Requests;

public class DoctorTimeOffRequestRepository
{
    private const string FilePath = "../../../Data/doctorTimeOffRequests.csv";
    private static DoctorTimeOffRequestRepository? _instance;

    public static DoctorTimeOffRequestRepository Instance => _instance ??= new DoctorTimeOffRequestRepository();

    public List<DoctorTimeOffRequest> GetAll()
    {
        return Serializer<DoctorTimeOffRequest>.FromCSV(FilePath);
    }

    public DoctorTimeOffRequest? GetById(string id)
    {
        return GetAll().Find(request => request.Id == id);
    }

    public void Add(DoctorTimeOffRequest request)
    {
        var allRequests = GetAll();

        allRequests.Add(request);

        Serializer<DoctorTimeOffRequest>.ToCSV(allRequests, FilePath);
    }

    public void Update(DoctorTimeOffRequest request)
    {
        var allRequests = GetAll();

        var indexToUpdate = allRequests.FindIndex(e => e.Id == request.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allRequests[indexToUpdate] = request;

        Serializer<DoctorTimeOffRequest>.ToCSV(allRequests, FilePath);
    }

    public void Delete(DoctorTimeOffRequest request)
    {
        var allRequests = GetAll();

        var indexToDelete = allRequests.FindIndex(e => e.Id == request.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allRequests.RemoveAt(indexToDelete);

        Serializer<DoctorTimeOffRequest>.ToCSV(allRequests, FilePath);
    }

    public static void DeleteAll()
    {
        var emptyDoctorTimeOffRequestList = new List<DoctorTimeOffRequest>();
        Serializer<DoctorTimeOffRequest>.ToCSV(emptyDoctorTimeOffRequestList, FilePath);
    }
}