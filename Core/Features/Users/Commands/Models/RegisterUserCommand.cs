using Core.Bases;
using Core.Features.Users.Commands.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Features.Users.Commands.Models
{
    public class RegisterUserCommand : IRequest<Response<UserResponse>>
    {


        public string UserName { get; set; }


        //[PhoneValidation]
        public string Phone { get; set; }


        public string Email { get; set; }


        //[GenderValidation]
        public string Gender { get; set; }


        //[UserTypeValidation]
        public string UserType { get; set; }


        public string Password { get; set; }


        public string ConfirmPassword { get; set; }

        public IFormFile? imgae { get; set; }



    }
}
