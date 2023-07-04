using System;
using System.Collections.Generic;
using System.IO;
using Hospital.Models.Books;
using Hospital.Serialization;

namespace Hospital.Repositories.Copys;

public class CopyRepository
{
    private const string FilePath = "../../../Data/Copies.json";
    private readonly ISerializer<Copy> _serializer;

    public CopyRepository(ISerializer<Copy> serializer)
    {
        _serializer = serializer;
    }

    public List<Copy> GetAll()
    {
        try
        {
            return _serializer.Load(FilePath);
        }
        catch (Exception ex)
        {
            if (ex is FileNotFoundException)
                return new List<Copy>();
            throw;
        }
    }

    public Copy? GetByInventoryNumber(string inventoryNumber)
    {
        return GetAll().Find(copy => copy.InventoryNumber == inventoryNumber);
    }

    public void Add(Copy copy)
    {
        var allCopy = GetAll();

        allCopy.Add(copy);

        _serializer.Save(allCopy, FilePath);
    }

    public void Update(Copy copy)
    {
        var allCopy = GetAll();

        var indexToUpdate = allCopy.FindIndex(e => e.InventoryNumber == copy.InventoryNumber);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allCopy[indexToUpdate] = copy;

        _serializer.Save(allCopy, FilePath);
    }

    public void Delete(Copy copy)
    {
        var allCopy = GetAll();

        var indexToDelete = allCopy.FindIndex(e => e.InventoryNumber == copy.InventoryNumber);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allCopy.RemoveAt(indexToDelete);

        _serializer.Save(allCopy, FilePath);
    }

    public void DeleteAll()
    {
        var emptyCopyList = new List<Copy>();
        _serializer.Save(emptyCopyList, FilePath);
    }
}