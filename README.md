# Predmetni projekat iz predmeta SIMS

    Implementacija projektnog zadatka radjena je u C# jeziku, oslanjajuci se pretezno 
    na MVVM sablon u WPF-u

    Aplikaciji je moguce pristupiti preko dve razlicite uloge:
      - Bibliotekar
      - Clan

    Implementirane funkcionalnosti: 
      - logovanje na sistem
      - CRUD knjiga
      - CRUD primeraka knjiga
      - CRUD clanova biblioteke
      - CRUD pozajmica
      - Izvestaj top 10 najcitanijih knjiga

## Logovanje na sistem
    Logovanje se vrsi preko jednostavne forme u kojoj se zahteva unos korisnickog imena i lozinke

<div align="center">
  <img src="img/image.png" alt="Login forma" />
</div>

## Bibliotekar:
    Ukoliko je ulogovani korisnik bibliotekar, funkcionalnosti koje on moze da izvrsava su vezane za 
    upravljanje clanovima biblioteke, upravljanje katalogom i upravljanje pozajmicama
    
### Izgled prozora bibiliotekara:
<div align="center">
  <img src="img/image2.png" alt="Upravljanje clanovima" />
  <img src="img/image3.png" alt="Upravljanje primercima" />
  <img src="img/image4.png" alt="Upravljanje naslovima" />
  <img src="img/image9.png" alt="Upravljanje pozajmicama" />
</div>

## Clan
    Ukoliko je ulogovani korisnik clan biblioteke, funkcionalnosti koje on moze da izvrsava su vezane za 
    pretragu kataloga, pregled iznajmljenih knjiga, kao i uvid u najcitanije naslove biblioteke

### Izgled prozora clana biblioteke:
<div align="center">
  <img src="img/image5.png" alt="Upravljanje naslovima" />
</div>

## Primer detaljnog prikaza informacija o knjizi i kreiranje pozajmice:
<div align="center">
  <img src="img/image6.png" alt="Detaljne informacije knjige"/>
  <img src="img/image7.png" alt="Kreiranje pozajmice" />
</div>

## Prikaz najcitanijih naslova:

<div align="center">
  <img src="img/image8.png" alt="Najcitaniji naslovi" />
</div>
