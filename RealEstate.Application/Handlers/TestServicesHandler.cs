using MediatR;
using RealEstate.Application.Commands;
using RealEstate.Application.Services;
using System.Threading.Tasks;

namespace RealEstate.Application.Handlers
{
   public class TestServicesHandler : IRequestHandler<TestRequest, TestReponse>
    {
        private readonly IExturnalTestService _exturnalTestService;
        public TestServicesHandler(IExturnalTestService exturnalTestService)
        {
            _exturnalTestService = exturnalTestService;
        }
        public Task<TestReponse> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            TestReponse testReponse = new TestReponse();
            _exturnalTestService.ExturnalTest();
            return Task.FromResult(testReponse);
        }
    }
}