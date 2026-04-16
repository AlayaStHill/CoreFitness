# CoreFitness

## Projektbeskrivning

Detta projekt är en fullstack webbapplikation utvecklad i ASP.NET Core MVC.

Applikationen fungerar som en webbportal för ett gym där användare kan:

- Registrera och logga in på ett konto  
- Skapa och hantera medlemskap  
- Se tillgängliga pass  
- Boka och avboka pass  
- Se sin egen information på "My Account"  

Systemet innehåller även en admin-del där administratörer kan hantera pass.

Projektet är uppbyggt enligt principerna för:

- Domain-Driven Design (DDD)  
- Clean Architecture 

---

## Arkitektur & Struktur

Projektet är uppdelat i flera lager för att skapa en tydlig och testbar struktur:

### Domain
Innehåller affärslogik, entiteter och value objects  

### Application
Innehåller use cases och affärsflöden  

### Infrastructure
Hanterar databas (Entity Framework Core) och externa tjänster  

### Presentation (MVC)
Controllers, Views och UI-logik (inkl. HTMX)  

---

## Funktionalitet

### Användare
- Registrering via ASP.NET Core Identity  
- Inloggning och utloggning  
- Extern inloggning (GitHub)  
- Hantering av profil  

### Medlemskap
- Skapa medlemskap  
- Visa medlemskapsstatus
- Pausa medlemsskap
- Återaktivera medlemsskap  

### Pass (Workout Sessions)
- Visa lista över pass  

Innehåller:
- Namn  
- Datum & tid  
- Typ/kategori  
- Kapacitet  

### Bokningar
- Boka pass  
- Avboka pass  
- Förhindrar dubbelbokning  

### Admin
- Skapa pass  
- Ta bort pass  

---

## Säkerhet

- ASP.NET Core Identity för autentisering  
- Rollbaserad behörighet:
  - Admin  
  - Member  
- Skyddade endpoints med `[Authorize]`  
- Skydd mot CSRF-attacker via Anti-Forgery Tokens i formulär  
- Validering via ModelState  

---

## Databas

- Entity Framework Core (Code First)  

### Miljöer
- SQLite används i utvecklingsmiljö och skapas automatiskt vid uppstart
- SQLite In-Memory används i integrationstester
- SQL Server används i produktionsmiljö

I produktionsmiljö tillämpas migreringar automatiskt vid uppstart av applikationen.

--- 

## Tester

Projektet innehåller:

### Enhetstester
- xUnit & NSubstitute  
Testar domänlogik och affärsregler  

### Integrationstester
- SQLite In-Memory används  
Testar applikationsflöden 

--- 

## Tekniker & Verktyg

- ASP.NET Core MVC  
- Entity Framework Core  
- ASP.NET Core Identity  
- HTMX används i admin-delen för dynamiska uppdateringar utan full page reload  
- xUnit  
- NSubstitute  
- Git & GitHub 

---

## Starta projektet lokalt

### 1. Klona repositoryt

```bash
git clone https://github.com/AlayaStHill/CoreFitness.git
```

### 2. Databas

I utvecklingsmiljö skapas SQLite-databasen automatiskt vid uppstart av applikationen.

### 3. Starta applikationen

Tryck på Run i Visual Studio eller:
```bash
dotnet run
```

### 4. Öppna i webbläsaren

Applikationen startar i utvecklingsläge på:

https://localhost:7001
