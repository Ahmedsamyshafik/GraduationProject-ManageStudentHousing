using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? College { get; set; }
        public string? University { get; set; }
        public string? Address { get; set; }
        public string Gender { get; set; }
        public string? imageUrl { get; set; }
        public string? ImageName { get; set; }
        public int Counter { get; set; }
        public string CodeConfirm { get; set; }

        public virtual ICollection<Apartment>? OwnedApartment { get; set; }//Owner
        public virtual UsersApartments? CurrentLivingIn { get; set; } // for Studnets


        public virtual ICollection<View>? Viewers { get; set; }
        public virtual ICollection<UserApartmentsComment>? Comments { get; set; }
        public virtual ICollection<UserApartmentsReact>? Reacts { get; set; }



    }
}
