using AutoMapper;
using BCrypt.Net;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Korisnik;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;

namespace Knjizara.Data
{
    public class KorisnikRepository : IKorisnikRepository
    {
        private readonly KnjizaraDBContext context;
        private readonly IMapper mapper;

        public KorisnikRepository(KnjizaraDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public KorisnikConfirmation AddKorisnik(Korisnik korisnik)
        {
            var createdKorisnik = context.Add(korisnik);
            return mapper.Map<KorisnikConfirmation>(createdKorisnik.Entity);
        }

        public List<Korisnik> GetKorisnik()
        {
            List<Korisnik> korisnik = context.korisnik.ToList();
            return korisnik;
        }

        public Korisnik GetKorisnikId(Guid Id)
        {
            return context.korisnik.FirstOrDefault(e => e.id_korisnik == Id);
        }

        public void RemoveKorisnik(Guid Id)
        {
            var korisnik = GetKorisnikId(Id);
            context.Remove(korisnik);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public Korisnik UpdateKorisnik(Korisnik korisnik)
        {
            try
            {
                var existingKorisnik = context.korisnik.FirstOrDefault(e => e.id_korisnik == korisnik.id_korisnik);

                if (existingKorisnik != null)
                {
                    // Update the existing backlog with the new values
                    existingKorisnik.ime_korisnika = korisnik.ime_korisnika;
                    existingKorisnik.prezime_korisnika = korisnik.prezime_korisnika;
                    existingKorisnik.adresa_korisnika = korisnik.adresa_korisnika;
                    existingKorisnik.grad_korisnika = korisnik.grad_korisnika;
                    existingKorisnik.kontakt_telefon = korisnik.kontakt_telefon;
                    existingKorisnik.status_korisnika = korisnik.status_korisnika;

                    context.SaveChanges(); // Save changes to the database

                    return existingKorisnik;
                }
                else
                {
                    throw new KeyNotFoundException($"Korisnik with ID {korisnik.id_korisnik} not found");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error updating korisnik", ex);
            };
        }

        public Korisnik GetKorisnikByUsernameAndPassword(string username, string password)
        {
            List<Korisnik> korisnik = context.korisnik.ToList();
           
           for(int i = 0; i < korisnik.Count -1; i++)
            {
                if (korisnik[i].username == username) {
                
                    if (BCrypt.Net.BCrypt.Verify(password, korisnik[i].password))
                    {
                        return korisnik[i];
                    }
                }
            }
            return null;
            //return context.korisnik.FirstOrDefault(e => e.username == username && BCrypt.Net.BCrypt.Verify(password, e.pasword));
        }


        public Korisnik GetKorisnikByUsername(string username)
        {
            List<Korisnik> korisnik = context.korisnik.ToList();

            for (int i = 0; i < korisnik.Count - 1; i++)
            {
                if (korisnik[i].username == username)
                {


                    return korisnik[i];

                }
            }
            return null;
            //return context.korisnik.FirstOrDefault(e => e.username == username && BCrypt.Net.BCrypt.Verify(password, e.pasword));
        }


        public Task<bool> CheckUsernameExistAsync(string? username)
        {
            return context.korisnik.AnyAsync(x => x.username == username);
            //throw new NotImplementedException();
        }

        public async Task<bool> CheckFullNameExistAsync(string firstName, string lastName)
        {

            return await context.korisnik.AnyAsync(x => x.ime_korisnika == firstName && x.prezime_korisnika == lastName);

        }


        public string CheckPasswordStrength(string pass)
        {
            StringBuilder sb = new StringBuilder();
            if (pass.Length < 6)
                sb.Append("Minimum password length should be 5" + Environment.NewLine);
            if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
                sb.Append("Password should be AlphaNumeric" + Environment.NewLine);
            // if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
            //    sb.Append("Password should contain special charcter" + Environment.NewLine);
            return sb.ToString();
        }
    }
}
