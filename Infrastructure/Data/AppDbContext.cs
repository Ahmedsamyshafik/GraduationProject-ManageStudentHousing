using Domin.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Apartment> Apartment { get; set; }
        public DbSet<UsersApartments> UsersApartments { get; set; }
        public DbSet<ApartmentImages> apartmentImages { get; set; }
        public DbSet<RoyalDocument> royalDocuments { get; set; }
        public DbSet<ApartmentVideo> apartmentVideos { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<View> views { get; set; }
        public DbSet<UserApartmentsReact> userApartmentsReacts { get; set; }
        public DbSet<UserApartmentsComment> userapartmentsComments { get; set; }
        public DbSet<UserApartmentsRequests> UserApartmentsRequests { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region FK(Apartment)

            modelBuilder.Entity<Apartment>()
                .HasOne(r => r.RoyalDocument)
                .WithOne(a => a.Apartment)
                .HasForeignKey<RoyalDocument>(r => r.ApartmentID) // in each <apartment> we put Royal Document id  & when delete apartment delete also it's royal doc
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Apartment>()
                .HasOne(v => v.ApartmentVideo)
                .WithOne(a => a.Apartment)
                .HasForeignKey<ApartmentVideo>(a => a.ApartmentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Apartment>()
                .HasMany(i => i.ApartmentImages)
                .WithOne(a => a.Apartment)
                .HasForeignKey(a => a.ApartmentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Apartment>()
                .HasOne(o => o.Owner)
                .WithMany(a => a.OwnedApartment)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.NoAction); // when delete apartment don't do this to Owner !

            modelBuilder.Entity<Apartment>()
                .HasMany(s => s.StudentsApartment)
                .WithOne(a => a.ApartmentFK)
                .HasForeignKey(a => a.ApartmentID)
                .OnDelete(DeleteBehavior.Restrict);//Can't Delete apartment if it have already students in it 

            modelBuilder.Entity<Apartment>()
                .HasMany(r => r.Reacts)
                .WithOne(a => a.Apartment)
                .HasForeignKey(a => a.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);//when apartment deleted delete all reacts

            modelBuilder.Entity<Apartment>()
                .HasMany(c => c.Comments)
                .WithOne(a => a.Apartment)
                .HasForeignKey(a => a.ApartmentId)
                .OnDelete(DeleteBehavior.Cascade);//when apartment deleted delete all comments



            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.OwnedApartment)
                .WithOne(o => o.Owner)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);


            #endregion

            #region FK(ApartmentImages)
            modelBuilder.Entity<ApartmentImages>()
                .HasOne(x => x.Apartment)
                .WithMany(x => x.ApartmentImages)
                .HasForeignKey(x => x.ApartmentID)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region FK(ApartmentVideo)
            
            modelBuilder.Entity<ApartmentVideo>()
                .HasOne(a=>a.Apartment)
                .WithOne(a=>a.ApartmentVideo)
                .HasForeignKey<Apartment>(x=>x.ApartmentVideoID)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region FK(RoyalDocument)

            modelBuilder.Entity<RoyalDocument>()
                .HasOne(a => a.Apartment)
                .WithOne(a => a.RoyalDocument)
                .HasForeignKey<Apartment>(x => x.RoyalDocumentID)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region FK(UserApartmentReact)

            modelBuilder.Entity<UserApartmentsReact>()
                .HasOne(u=>u.user)
                .WithMany(r=>r.Reacts)
                .HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.NoAction); // when delete this colum not delete user

            modelBuilder.Entity<UserApartmentsReact>()
             .HasOne(u => u.Apartment)
             .WithMany(r => r.Reacts)
             .HasForeignKey(x => x.ApartmentId)
             .OnDelete(DeleteBehavior.NoAction);// when delete this colum not delete apartment

            #endregion

            #region FK(UserApartmentComments)

            modelBuilder.Entity<UserApartmentsComment>()
              .HasOne(u => u.user)
              .WithMany(r => r.Comments)
              .HasForeignKey(x => x.UserId)
              .OnDelete(DeleteBehavior.NoAction); // when delete this colum not delete user

            modelBuilder.Entity<UserApartmentsComment>()
             .HasOne(u => u.Apartment)
             .WithMany(r => r.Comments)
             .HasForeignKey(x => x.ApartmentId)
             .OnDelete(DeleteBehavior.NoAction);// when delete this colum not delete apartment

            #endregion

            #region FK(UserApartment)

            modelBuilder.Entity<UsersApartments>()
                .HasOne(a=>a.ApartmentFK)
                .WithMany(s=>s.StudentsApartment)
                .HasForeignKey(x=>x.ApartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsersApartments>()
                .HasMany(u => u.Users)
                .WithOne(s => s.CurrentLivingIn) // no fk
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region FK(UserApartmentRequests)
            
            modelBuilder.Entity<UserApartmentsRequests>()
                .HasOne(x=>x.ApplicationUser)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserApartmentsRequests>()
               .HasOne(x => x.Apartment)
               .WithOne()
               .OnDelete(DeleteBehavior.NoAction);


            #endregion

            #region FK(Application User)

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a=>a.OwnedApartment)
                .WithOne(o=>o.Owner)
                .HasForeignKey(x=>x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.CurrentLivingIn)
                .WithMany(a => a.Users)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(x => x.Viewers)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);


            #endregion

  

            //Notification Transaction -View - (userData,Questionaire)






            base.OnModelCreating(modelBuilder);
        }


    }
}
