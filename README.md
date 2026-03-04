# 🔧 MatematijaAPI — Backend (.NET 8)

REST API za MatemaTI&JA platformu.

## ⚙️ Zahtjevi

- **.NET 8 SDK** — https://dotnet.microsoft.com/download/dotnet/8
- **SQL Server Express** (već imaš na `.\SQLEXPRESS`)
- **Visual Studio 2022** ili **VS Code** sa C# ekstenzijom

---

## 🚀 Pokretanje

### 1. Otvori terminal u folderu `matematija-backend`

```bash
cd matematija-backend
```

### 2. Instaliraj zavisnosti

```bash
dotnet restore
```

### 3. Pokreni (baza se kreira automatski!)

```bash
dotnet run
```

API se pokreće na: **http://localhost:5062**  
Swagger dokumentacija: **http://localhost:5062/swagger**

---

## 🗄️ Baza podataka u SSMS

**Server name:** `.\SQLEXPRESS`  
**Authentication:** Windows Authentication  
**Database name:** `MatematijaDB`

Baza se **automatski kreira** pri prvom pokretanju aplikacije.  
Ne trebaš ništa ručno da radiš u SSMS!

### Tabele koje će biti kreirane:
| Tabela | Sadržaj |
|--------|---------|
| `Korisnici` | Svi korisnici sistema |
| `TipoviCasova` | 4 tipa časa (automatski popunjeno) |
| `Termini` | Termini koje profesor postavlja |
| `Rezervacije` | Zakazani časovi učenika |

---

## 🔑 Podrazumjevani nalozi (automatski kreirani)

| Email | Lozinka | Uloga |
|-------|---------|-------|
| admin@matematija.rs | Admin123! | Admin |
| profesor@matematija.rs | Admin123! | Profesor |

---

## 📋 API Endpointi

### Auth (bez prijave)
```
POST /api/auth/registracija   — Registracija novog korisnika
POST /api/auth/prijava        — Prijava, vraca JWT token
```

### Casovi (bez prijave)
```
GET  /api/casovi              — Svi tipovi casova
GET  /api/casovi/{id}         — Jedan tip casa
POST /api/casovi              — Dodaj tip (samo Admin)
DEL  /api/casovi/{id}         — Deaktiviraj tip (samo Admin)
```

### Termini (bez prijave za citanje)
```
GET  /api/termini             — Termini (opcionalno ?datum=2025-02-20)
GET  /api/termini/slobodni    — Slobodni dani (?mesec=2025-02)
POST /api/termini             — Dodaj termin (Profesor/Admin)
DEL  /api/termini/{id}        — Obrisi termin (Profesor/Admin)
```

### Rezervacije (potrebna prijava)
```
GET  /api/rezervacije/moje    — Moji casovi (Ucenik)
GET  /api/rezervacije/sve     — Sve rezervacije (Profesor/Admin)
POST /api/rezervacije         — Zakaži čas (Ucenik)
DEL  /api/rezervacije/{id}    — Otkaži čas (48h pravilo!)
PUT  /api/rezervacije/{id}/ocena — Ocijeni završen čas
```

### Korisnici (potrebna prijava)
```
GET  /api/korisnici/profil    — Moj profil
GET  /api/korisnici           — Svi korisnici (samo Admin)
DEL  /api/korisnici/{id}      — Obrisi korisnika (samo Admin)
```

---

## 🛡️ JWT Autentifikacija

Token se dobija pri prijavi/registraciji.  
Šalje se u header-u svakog zahtjeva:

```
Authorization: Bearer <token>
```

Token traje **7 dana**.

---

## 🔗 Povezivanje sa Frontendoм

Frontend React radi na `http://localhost:3000`  
Backend .NET radi na `http://localhost:5062`

CORS je već podešen — React može da komunicira sa API-jem.

---

*Projekat: Softversko inženjerstvo I — Državni univerzitet u Novom Pazaru*
