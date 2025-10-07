using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyArchive.Domain.Entities;

public class Family
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!; // e.g., "Smith Family"
    public string? Description { get; set; }
    //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    //public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    // Members in the family
    public ICollection<Member> Members { get; set; } = new List<Member>();
}
