using System;
using BCrypt.Net;

namespace HashPasswordGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string motDePasse = "admin";
            string motDePasseHache = HacheurMotDePasse.HacherMotDePasse(motDePasse);
            Console.WriteLine($"Mot de passe haché : {motDePasseHache}");
        }
    }

    public class HacheurMotDePasse
    {
        public static string HacherMotDePasse(string motDePasse)
        {
            return BCrypt.Net.BCrypt.HashPassword(motDePasse);
        }
    }
}
