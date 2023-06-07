using Hospital.Exceptions;
using Hospital.Models.Doctor;
using Hospital.Models.Manager;
using Hospital.Models.Nurse;
using Hospital.Models.Patient;
using Hospital.Services;
using System.Net;
using System.Security.Principal;

namespace HospitalCLI.Login
{
    public class LoginCli
    {
        private readonly LoginService _loginService = new();
        private string? _username;
        private string? _password;

        public void LoginUser()
        {
            Console.Write("Username: ");
            _username = Console.ReadLine()?.Trim();

            Console.Write("Password: ");
            _password = ReadPassword();

            ExecuteLogin();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(GetIdentity()), null);
        }

        public string ReadPassword()
        {
            var password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (!char.IsLetterOrDigit(key.KeyChar) && key.Key != ConsoleKey.Backspace) continue;

                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Remove(password.Length - 1);
                    Console.Write("\b \b");
                }
                else if (char.IsLetterOrDigit(key.KeyChar))
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        private void ExecuteLogin()
        {
            if (_loginService.AuthenticateUser(new NetworkCredential(_username, _password)))
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(GetIdentity()), null);
            }
            else
            {
                _loginService.LoggedUser = null;
            }
        }

        private string GetIdentity()
        {
            if (_loginService.LoggedUser == null) return "";

            var userId = _loginService.LoggedUser.Id;
            return $"{userId}|{GetUserType()}";
        }

        private string GetUserType()
        {
            if (_loginService.LoggedUser?.GetType() == typeof(Patient)) return "PATIENT";
            if (_loginService.LoggedUser?.GetType() == typeof(Doctor)) return "DOCTOR";
            if (_loginService.LoggedUser?.GetType() == typeof(Nurse)) return "NURSE";
            if (_loginService.LoggedUser?.GetType() == typeof(Manager)) return "MANAGER";

            throw new UnrecognizedUserTypeException();
        }
    }
}
