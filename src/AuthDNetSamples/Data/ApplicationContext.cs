using AuthDNetLib.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthDNetSamples.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : ApplicationDbContext(options)
{
}