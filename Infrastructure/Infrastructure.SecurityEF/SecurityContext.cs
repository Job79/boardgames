using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SecurityEF;

public class SecurityContext : IdentityDbContext
{
        public SecurityContext(DbContextOptions<SecurityContext> options) : base(options)
        {
        }
}