using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure
{
    public class StayFinderDbContext : IdentityDbContext<IdentityUser>
    {
        public StayFinderDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
