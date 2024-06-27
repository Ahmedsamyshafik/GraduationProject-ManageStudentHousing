using Core.Bases;
using MediatR;

namespace Core.Features.Apartments.Commands.Models
{
    public class DeleteApartmentCommand : IRequest<Response<string>>
    {
        public int ApartmentId { get; set; }

        public string userID { get; set; }

    }
}
