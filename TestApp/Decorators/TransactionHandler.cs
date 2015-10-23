using System;

namespace TestApp
{
    public class TransactionHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _innerHandler;
        public TransactionHandler(IRequestHandler<TRequest, TResponse> innerHandler)
        {
            if (innerHandler == null)
                throw new ArgumentNullException(nameof(innerHandler));
            _innerHandler = innerHandler;
        }

        public TResponse Handle(TRequest request)
        {
            Console.WriteLine("Decoration: fake transaction started");
            var response = _innerHandler.Handle(request);
            Console.WriteLine("Decoration: fake transaction ended");
            return response;
        }
    }
}