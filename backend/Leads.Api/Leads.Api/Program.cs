using Leads.Api.Data;
using Leads.Api.Services;
using Microsoft.EntityFrameworkCore;
using System.IO;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p => p
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});


builder.Services.AddScoped<IEmailService, FakeEmailService>();


void LimparPastasDeEmail()
{
    var baseDir = AppContext.BaseDirectory;           
    var contentRoot = builder.Environment.ContentRootPath; 

    string[] alvos = new[]
    {
        Path.Combine(baseDir,     "emails"),
        Path.Combine(baseDir,     "outbox"),
        Path.Combine(contentRoot, "emails"),
        Path.Combine(contentRoot, "outbox"),
    };

    int apagados = 0;
    foreach (var dir in alvos)
    {
        if (!Directory.Exists(dir)) continue;

        foreach (var arq in Directory.GetFiles(dir, "*.txt"))
        {
            File.Delete(arq);
            apagados++;
        }

       
        foreach (var sub in Directory.GetDirectories(dir))
        {
            Directory.Delete(sub, true);
        }
    }

    Console.WriteLine($"Limpeza concluida. Arquivos apagados: {apagados}");
}


LimparPastasDeEmail();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseCors();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await DataSeeder.SeedAsync(db);
}

app.Run();
