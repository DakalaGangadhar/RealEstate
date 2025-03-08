using MediatR;
using RealEstate.Application.Commands;
using System.Threading.Tasks;

namespace RealEstate.Application.Handlers
{
   public class TestServicesHandler : IRequestHandler<TestRequest, TestReponse>
    {
        public Task<TestReponse> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            TestReponse testReponse = new TestReponse();
            return Task.FromResult(testReponse);
        }
    }
}