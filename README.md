🛒 ECommerce Webshop

Een volledig functionele e-commerce webshop gebouwd met ASP.NET Core MVC 9.0.
Dit project demonstreert een moderne online winkel met gebruikersbeheer, productcatalogus, winkelwagen en checkout-functionaliteit.

📑 Inhoudsopgave


Features
Technologieën
Installatie
Gebruik
Projectstructuur
Database Schema
Testaccounts
Seed Data
Design & Beveiliging
Toekomstige Verbeteringen
Licentie
Contact

🎯 Overzicht

ECommerce Webshop is een volledig werkende online winkelapplicatie ontwikkeld met ASP.NET Core MVC.
De applicatie biedt een complete shoppingervaring met gebruikersregistratie, productbeheer, winkelwagenfunctionaliteit en een admin dashboard voor beheer.

✨ Features
👥 Voor Klanten

Gebruikersbeheer
Account aanmaken, inloggen en uitloggen
Persoonlijke gegevens beheren
Product Browsing
Overzicht van alle producten
Filteren op categorie
Zoekfunctionaliteit
Gedetailleerde productpagina’s
Winkelwagen & Checkout
Producten toevoegen, verwijderen of aanpassen
Real-time totaalberekening
Veilig checkout-formulier
Meerdere betaalmethoden (iDEAL, Credit Card, PayPal)
Orderbevestiging en voorraadupdate

🛠️ Voor Administrators

Admin Dashboard
Statistieken: gebruikers, bestellingen, omzet
Recente bestellingen
Gebruikersbeheer
Gebruikerslijst, details, bestelgeschiedenis
Verwijderfunctie met bescherming
Orderbeheer
Overzicht van alle bestellingen
Orderdetails en statusbeheer

💻 Algemeen

Volledig responsive design (Bootstrap 5)
Moderne UI met gradients en animaties
Informatieve pagina’s: Over ons, FAQ, Verzending, Voorwaarden, Privacybeleid

🧩 Technologieën

Backend
ASP.NET Core MVC 9.0
Entity Framework Core 9.0
ASP.NET Core Identity
In-Memory Database (voor demo/doeleinden)
Frontend
Bootstrap 5
jQuery 3.x + jQuery Validation
Bootstrap Icons
Google Fonts (Poppins)
Unsplash (productafbeeldingen)

NuGet Packages
Microsoft.AspNetCore.Identity.EntityFrameworkCore (9.0.0)
Microsoft.AspNetCore.Identity.UI (9.0.0)
Microsoft.AspNetCore.Session (2.2.0)
Microsoft.EntityFrameworkCore.InMemory (9.0.0)
Microsoft.EntityFrameworkCore.SqlServer (9.0.0)
Microsoft.EntityFrameworkCore.Tools (9.0.0)
Newtonsoft.Json (13.0.3)

📥 Installatie
Vereisten

.NET 9.0 SDK
Visual Studio 2022
Git (optioneel)

Stappen
# Clone de repository
git clone <repository-url>
cd ECommerceWebshop

# Open project
start ECommerceWebshop.sln                          

# Herstel packages en build
dotnet restore
dotnet build

# Start de applicatie
dotnet run


Applicatie beschikbaar op:
🔗 https://localhost:7298
🔗 http://localhost:5135

🚀 Gebruik
Eerste opstart

Bij het eerste opstarten worden automatisch:
Database (In-Memory) aangemaakt
6 categorieën toegevoegd
33 producten geladen
2 testaccounts aangemaakt

Inloggegevens
👑 Admin
Email: admin@shopbe.com
Wachtwoord: Admin123!


Toegang tot dashboard, gebruikers en orderbeheer.

👤 Gebruiker
Email: user@shopbe.com
Wachtwoord: User123!


Toegang tot productcatalogus, winkelwagen, checkout en profiel.

🕵️ Gast

Kan producten bekijken, maar niet afrekenen zonder account.

📁 Projectstructuur
ECommerceWebshop/
├── Controllers/
│   ├── AccountController.cs
│   ├── AdminController.cs
│   ├── CartController.cs
│   ├── HomeController.cs
│   └── ProductController.cs
├── Models/
│   ├── ApplicationUser.cs
│   ├── Product.cs
│   ├── Category.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   └── CartItem.cs
├── ViewModels/
│   ├── LoginViewModel.cs
│   └── RegisterViewModel.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   └── IdentityDataSeeder.cs
├── Helpers/
│   └── SessionHelper.cs
├── Views/
│   ├── Account/
│   ├── Admin/
│   ├── Cart/
│   ├── Home/
│   ├── Product/
│   └── Shared/
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
└── Program.cs

🗄️ Database Schema

Relaties:

Category → Products (One-to-Many)
Order → OrderItems (One-to-Many)
Product → OrderItems (One-to-Many)

Belangrijkste tabellen:
Users, Products, Categories, Orders, OrderItems

👥 Testaccounts
Rol	Email	Wachtwoord	Functies
Admin	admin@shopbe.com
	Admin123!	Dashboard, gebruikers- & orderbeheer
User	user@shopbe.com
	User123!	Producten bekijken, bestellen, profiel
📊 Seed Data

Categorieën (6)
📱 Elektronica
👕 Kleding 
📚 Boeken  
⚽ Sport 
🏠 Huis & Tuin 
🎮 Speelgoed

Producten (33)
Elk met realistische prijzen, voorraad en afbeeldingen
Bijv. Laptop Pro 15” (€1299.99) · Yoga Mat Pro (€39.99)

🎨 Design & Beveiliging

Design

Modern gradient design
Responsive layout (desktop, tablet, mobiel)
Card-based UI en hover-animaties

Beveiliging

ASP.NET Core Identity
Password hashing
Anti-forgery tokens
Role-based authorization
SQL Injection & XSS bescherming

🚧 Toekomstige Verbeteringen

SQL Server database-implementatie
Productreviews en ratings
Wishlist functionaliteit
Geavanceerde filters
Order tracking
Productvoorraadbeheer
Exportfuncties (Excel/PDF)
Gebruikersstatistieken