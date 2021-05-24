using ContaAPI.Domain.Entities;
using ContaAPI.Infra.Data.Mapping;
using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;

namespace ContaAPI.Infra.Data.Context
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<AccountEntity> Accounts { get; set; }

        public DbSet<HistoricEntity> Historic { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>(new UserMap().Configure);
            modelBuilder.Entity<AccountEntity>(new AccountMap().Configure);
            modelBuilder.Entity<HistoricEntity>(new HistoricMap().Configure);

            var entites = Assembly
                .Load("ContaAPI.Domain")
                .GetTypes()
                .Where(w => w.Namespace == "ContaAPI.Domain.Entities" && w.BaseType.BaseType == typeof(Notifiable));

            foreach (var item in entites)
            {
                modelBuilder.Entity(item).Ignore(nameof(Notifiable.Invalid));
                modelBuilder.Entity(item).Ignore(nameof(Notifiable.Valid));
                modelBuilder.Entity(item).Ignore(nameof(Notifiable.Notifications));
            }
        }
    }
}
