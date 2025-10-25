using Leads.Api.Models;

namespace Leads.Api.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!db.Leads.Any())
        {
            db.Leads.AddRange(
                new Lead { ContactFirstName = "Luis", Suburb = "Lagoinha", Category = "Informática", Description = "Instalação de software", Price = 100 },
                new Lead { ContactFirstName = "Clara", Suburb = "Castelo", Category = "Locação de carro", Description = "Locação com veículos confortáveis", Price = 11000 },
                new Lead { ContactFirstName = "Leonardo", Suburb = "Liberdade", Category = "Conserto de carro", Description = "Diagnóstico rápido e completo", Price = 300 }
            );
            await db.SaveChangesAsync();
        }
    }
}
