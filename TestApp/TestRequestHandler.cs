using System;

namespace TestApp
{
    public class TestRequestHandler : IRequestHandler<TestRequest, Unit>
    {
        public Unit Handle(TestRequest command)
        {
            Console.WriteLine("HANDLING THE TEST REQUEST: {0} ", command.Title);
            return Unit.Value;
        }
    }
}