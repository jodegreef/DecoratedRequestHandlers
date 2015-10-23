using Autofac;
using System;

namespace TestApp
{
    public class Mediator
    {
        private readonly IContainer _container;
        public Mediator(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
            _container = container;
        }

        public TResponse Send<TResponse>(IRequest<TResponse> request)
        {
            var handlerType = typeof (IRequestHandler<, >).MakeGenericType(request.GetType(), typeof (TResponse));
            dynamic handler = _container.Resolve(handlerType);
            return handler.Handle((dynamic)request);
        }
    }
}