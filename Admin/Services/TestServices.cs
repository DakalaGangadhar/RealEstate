using Common.Models;
using MediatR;
using RealEstate.Application.Commands;
using RealEstate.Application.Services;

namespace RealEstate.Admin.Services
{
    public class TestServices : ITestServices
    {
        public readonly IMediator _mediator;
        public TestServices(IMediator mediator)
        {
            _mediator = mediator;
        }
        public void TestData()
        {
            //throw new NotImplementedException();
            string s = "Hello Hi";
            TestRequest testRequest = new TestRequest();
            testRequest.Id = 234;
            _mediator.Send(testRequest);
        }
    }
}
