﻿using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Korisnik;

namespace Knjizara.Data
{
    public interface IKorisnikRepository
    {
        List<Korisnik> GetKorisnik();
        public Korisnik GetKorisnikId(Guid Id);
        public void RemoveKorisnik(Guid Id);
        public KorisnikConfirmation AddKorisnik(Korisnik korisnik);
        public Korisnik UpdateKorisnik(Korisnik korisnik);
        public bool SaveChanges();

        public Korisnik GetKorisnikByUsernameAndPassword(string username, string password);
        public Korisnik GetKorisnikByUsername(string username);
        public Task<bool> CheckUsernameExistAsync(string? username);
        public string CheckPasswordStrength(string pass);
        public Task<bool> CheckFullNameExistAsync(string firstName, string lastName);

    }
}
