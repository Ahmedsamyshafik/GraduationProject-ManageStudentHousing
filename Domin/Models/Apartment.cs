using System.ComponentModel.DataAnnotations;

namespace Domin.Models
{
    public class Apartment
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string City { get; set; }
        public string gender { get; set; }  
        public string? Description { get; set; }
        public int Room { get; set; }
        public decimal Price { get; set; }
        public int NumberOfUsers { get; set; }
        public int NumberOfUsersExisting { get; set; }
        public int Likes { get; set; }
        public bool Publish { get; set; }
        public string? CoverImageName { get; set; }
        public string? CoverImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public int RoyalDocumentID { get; set; }
        public virtual RoyalDocument RoyalDocument { get; set; }

        public int ApartmentVideoID { get; set; }
        public virtual ApartmentVideo ApartmentVideo { get; set; }
        
        public  virtual ICollection<ApartmentImages>? ApartmentImages { get; set; }
          
        public string OwnerId { get; set; } 
        public virtual ApplicationUser Owner { get; set; }
        
        public virtual ICollection<UsersApartments>? StudentsApartment { get; set; }
        public virtual ICollection<UserApartmentsReact>? Reacts { get; set; }
        public virtual ICollection<UserApartmentsComment>? Comments { get; set; }




    }
}
