namespace Leads.Api.Models;

public enum LeadStatus { New = 0, Accepted = 1, Declined = 2 }

public class Lead
{
    public int Id { get; set; }
    public string ContactFirstName { get; set; } = "";
    public string? ContactLastName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public string? Suburb { get; set; }
    public string? Category { get; set; }
    public string Description { get; set; } = "";
    public decimal Price { get; set; }

    public LeadStatus Status { get; set; } = LeadStatus.New;
}
