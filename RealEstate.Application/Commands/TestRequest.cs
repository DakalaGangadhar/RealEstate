using MediatR;

namespace RealEstate.Application.Commands
{
    public class TestRequest : IRequest<TestReponse>
    {
        public int Id { get; set; }
    }
}
