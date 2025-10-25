using System;
using System.IO;
using System.Threading.Tasks;

namespace Leads.Api.Services
{
    public class FakeEmailService : IEmailService
    {
        private readonly string _outboxDir;

        public FakeEmailService(IConfiguration cfg)
        {
          
            string baseDir = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;

            
            _outboxDir = Path.Combine(baseDir, "outbox");

            Directory.CreateDirectory(_outboxDir);

            Console.WriteLine($"📂 Caminho fixo da OUTBOX: {_outboxDir}");
        }

        public Task SendAsync(string to, string subject, string body)
        {
            
            var path = Path.Combine(_outboxDir, $"{DateTime.Now:yyyyMMdd_HHmmssfff}_{to.Replace("@", "_at_")}.txt");

            
            return File.WriteAllTextAsync(path, $"TO: {to}\nSUBJECT: {subject}\n\n{body}");
        }
    }
}
