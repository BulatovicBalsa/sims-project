using System.Collections.Generic;
using Hospital.Models.Manager;
using Hospital.Serialization;

namespace Hospital.Repositories.Manager;

public class RenovationRepository
{
    private const string FilePath = "../../../Data/renovations.csv";
    private static RenovationRepository? _instance;
    private List<Renovation>? _renovations;


    private RenovationRepository()
    {
    }

    public static RenovationRepository Instance => _instance ??= new RenovationRepository();

    private void JoinWithRooms(List<Renovation> renovations)
    {
        renovations.ForEach(renovation => renovation.Room = RoomRepository.Instance.GetById(renovation.RoomId) ??
                                                            throw new KeyNotFoundException(
                                                                "Room for renovation not found"));
    }

    public List<Renovation> GetAll()
    {
        return _renovations ?? GetAllFromFile();
    }

    public List<Renovation> GetAllFromFile()
    {
        _renovations = Serializer<Renovation>.FromCSV(FilePath);
        JoinWithRooms(_renovations);
        return _renovations;
    }

    public void Add(Renovation renovation)
    {
        _renovations = GetAll();
        _renovations.Add(renovation);
        Serializer<Renovation>.ToCSV(_renovations, FilePath);
    }
}