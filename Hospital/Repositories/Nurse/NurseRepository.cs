using System.Collections.Generic;
using System.Linq;
using Hospital.Exceptions;
using Hospital.Serialization;

namespace Hospital.Repositories.Nurse;

public class NurseRepository
{
    private const string FilePath = "../../../Data/nurses.csv";

    public List<Models.Nurse.Nurse> GetAll()
    {
        return Serializer<Models.Nurse.Nurse>.FromCSV(FilePath);
    }

    public Models.Nurse.Nurse? GetById(string id)
    {
        return GetAll().FirstOrDefault(nurse => nurse.Id == id);
    }

    public Models.Nurse.Nurse? GetByUsername(string username)
    {
        return GetAll().FirstOrDefault(nurse => nurse.Profile.Username == username);
    }

    public void Add(Models.Nurse.Nurse nurse)
    {
        var allNurses = GetAll();
        allNurses.Add(nurse);
        Serializer<Models.Nurse.Nurse>.ToCSV(allNurses, FilePath);
    }

    public void Update(Models.Nurse.Nurse nurse)
    {
        var allNurses = GetAll();

        var indexToUpdate = allNurses.FindIndex(nurseRecord => nurseRecord.Id == nurse.Id);
        if (indexToUpdate == -1)
            throw new ObjectNotFoundException($"Nurse with id {nurse.Id} was not found.");
        allNurses[indexToUpdate] = nurse;

        Serializer<Models.Nurse.Nurse>.ToCSV(allNurses, FilePath);
    }

    public void Delete(Models.Nurse.Nurse nurse)
    {
        var allNurses = GetAll();

        if (!allNurses.Remove(nurse))
            throw new ObjectNotFoundException($"Nurse with id {nurse.Id} was not found.");

        Serializer<Models.Nurse.Nurse>.ToCSV(allNurses, FilePath);
    }
}