using Microsoft.EntityFrameworkCore;

namespace SYS.Integra.src.SYS.Integra.Infraestructure
{
    public class ERPContext : DbContext
    {
        public ERPContext(DbContextOptions<ERPContext> options) : base(options)
        {
            
        }
    }
}
