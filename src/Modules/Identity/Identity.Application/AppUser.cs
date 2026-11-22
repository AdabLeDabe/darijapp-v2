using Microsoft.AspNetCore.Identity;

namespace Identity.Application;

public sealed class AppUser : IdentityUser<Guid>
{
    public string DisplayName { get; set; } = "";
    
    public DateTimeOffset CreatedAt { get; set; }
}