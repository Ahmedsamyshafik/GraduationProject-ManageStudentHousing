using Core.Bases;
using MediatR;


namespace Core.Features.Users.Commands.Models.PaymentRequest
{
    public class FormPaymentCommand :IRequest<Response<string>>
    {
        public int apartmentID { get; set; }
        public string UserID { get; set; }



    }
}
