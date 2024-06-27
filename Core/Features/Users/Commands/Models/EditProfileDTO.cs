using Microsoft.AspNetCore.Http;

namespace Core.Features.Users.Commands.Models
{
    public class EditProfileDTO
    {

        public string Name { get; set; }


        //[PhoneValidation]
        public string Phone { get; set; }



        public string Email { get; set; }

        //[MaxFileSize(1024 * 1024)]
        public IFormFile? Picture { get; set; }
    }
}
