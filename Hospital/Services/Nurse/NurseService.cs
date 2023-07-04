using Hospital.DTOs;
using System;
using System.Collections.Generic;
using System.Threading;
using Hospital.Models;
using Hospital.Repositories;

namespace Hospital.Services

{

    public class LibrarianService
    {
        private readonly LibrarianRepository _librarianRepository;
        public LibrarianService() 
        {
            _librarianRepository = LibrarianRepository.Instance;
        }
        public List<Librarian> GetAll()
        {
            return _librarianRepository.GetAll();
        }
        public List<PersonDTO> GetLibrariansAsPersonDTOsByFilter(string id, string searchText)
        {
            return _librarianRepository.GetLibrariansAsPersonDTOsByFilter(id, searchText);
        }

        public PersonDTO GetLoggedInLibrarian()
        {
            var identityName = Thread.CurrentPrincipal.Identity.Name;
            var id = identityName.Split("|")[0];
            var loggedInLibrarian = _librarianRepository.GetById(id);

            // Convert the Librarian object to PersonDTO
            return new PersonDTO(loggedInLibrarian.Id, loggedInLibrarian.FirstName, loggedInLibrarian.LastName, Role.Librarian);
        }
    }
}
