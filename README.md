# 🎮 WebOrderApp

A full-stack e-commerce web application for browsing and ordering video games, built with ASP.NET Core MVC and MySQL.

## 🚀 Features

- Browse a catalog of video games
- Place and manage orders
- Session-based cart system
- Responsive UI with Razor Views
- MySQL database with Entity Framework Core migrations

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | ASP.NET Core MVC (.NET 8) |
| Database | MySQL 8 + Entity Framework Core |
| Frontend | Razor Views, HTML, CSS, JavaScript |
| ORM | Entity Framework Core (Code First) |

## ⚙️ Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- MySQL 8.0+

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/KalifaDiarra/WebOrderApp.git
   cd WebOrderApp
   ```

2. **Configure the database**

   Create an `appsettings.json` file in the project root (not included for security reasons):
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "server=localhost;database=weborderapp;user=YOUR_USER;password=YOUR_PASSWORD"
     }
   }
   ```

3. **Apply migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the app**
   ```bash
   dotnet run
   ```
   Visit `http://localhost:5000`

## 📁 Project Structure

```
WebOrderApp/
├── Controllers/      # MVC Controllers
├── Models/           # Data models
├── Views/            # Razor view templates
├── Data/             # DbContext
├── Migrations/       # EF Core migrations
└── wwwroot/          # Static assets (CSS, JS)
```

## 👤 Author

**Kalifa Diarra** — [GitHub](https://github.com/KalifaDiarra)
