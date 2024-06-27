using Core.Bases;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Users.Commands.Models.ApartmentsRquests
{
    public class RequestApartmentStudentCommand : IRequest<Response<string>>
    {
        [Required]
        public string UserID { get; set; }

        [Required]
        public int ApartmentID { get; set; }

        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
