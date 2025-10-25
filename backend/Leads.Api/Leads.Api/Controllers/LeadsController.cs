using Leads.Api.Data;
using Leads.Api.Models;
using Leads.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Leads.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IEmailService _email;

    public LeadsController(AppDbContext db, IEmailService email)
    {
        _db = db;
        _email = email;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Lead>>> GetAll([FromQuery] LeadStatus? status)
    {
        var q = _db.Leads.AsQueryable();
        if (status.HasValue)
            q = q.Where(l => l.Status == status.Value);

        var list = await q.OrderByDescending(l => l.DateCreated).ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Lead>> GetById(int id)
    {
        var lead = await _db.Leads.FindAsync(id);
        return lead is null ? NotFound() : Ok(lead);
    }

    // POST /api/leads
    [HttpPost]
    public async Task<ActionResult<Lead>> Create([FromBody] Lead lead)
    {
        // defaults seguros
        if (lead.DateCreated == default) lead.DateCreated = DateTime.UtcNow;
        lead.Status = LeadStatus.New;

        _db.Leads.Add(lead);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = lead.Id }, lead);
    }

    // PUT /api/leads/{id}/accept
    [HttpPut("{id:int}/accept")]
    public async Task<IActionResult> Accept(int id)
    {
        var lead = await _db.Leads.FindAsync(id);
        if (lead is null) return NotFound();

        // Regra: > 500 aplica 10% off
        if (lead.Price > 500m)
            lead.Price = Math.Round(lead.Price * 0.9m, 2);

        lead.Status = LeadStatus.Accepted;
        await _db.SaveChangesAsync();

        // E-mail fake para vendas
        await _email.SendAsync(
            "vendas@test.com",
            $"Lead {lead.Id} aceito",
            $"O lead {lead.ContactFirstName} foi aceito. Preco final: {lead.Price:C}."
        );

        return NoContent();
    }

    // PUT /api/leads/{id}/decline
    [HttpPut("{id:int}/decline")]
    public async Task<IActionResult> Decline(int id)
    {
        var lead = await _db.Leads.FindAsync(id);
        if (lead is null) return NotFound();

        lead.Status = LeadStatus.Declined;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // (Opcional) DELETE /api/leads/clear-emails -> limpar outbox/emails sob demanda (Swagger)
    [HttpDelete("clear-emails")]
    public IActionResult ClearEmails()
    {
        var targets = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "emails"),
            Path.Combine(AppContext.BaseDirectory, "outbox"),
            Path.Combine(Directory.GetCurrentDirectory(), "emails"),
            Path.Combine(Directory.GetCurrentDirectory(), "outbox"),
        };

        int apagados = 0;
        foreach (var dir in targets)
        {
            if (!Directory.Exists(dir)) continue;
            foreach (var arq in Directory.GetFiles(dir, "*.txt"))
            {
                System.IO.File.Delete(arq);
                apagados++;
            }
        }

        return Ok(new { message = $"Limpeza concluida. Arquivos apagados: {apagados}" });
    }
}
