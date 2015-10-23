using Autofac;
using Autofac.Core;
using System;
using System.Linq;
using System.Reflection;

namespace TestApp
{
    /*
    THIS APPLICATION SHOWS HOW TO DECORATE REQUESTHANDLERS USING AUTOFAC
    */
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            var executingAssembly = Assembly.GetExecutingAssembly();
            //for debugging only
            var tasks = executingAssembly.GetTypes().Where(t => t.Name.EndsWith("Task", StringComparison.Ordinal)).ToList();
            builder.RegisterAssemblyTypes(executingAssembly)
                .As(type => type.GetInterfaces()
                .Where(interfaceType => interfaceType.IsClosedTypeOf(typeof (IRequestHandler<, >)))
                .Select(interfaceType => new KeyedService("requestHandler", interfaceType))); // --> we need a keyed service as the decorated service will become the key-less one

            //register the decorator - works
            builder.RegisterGenericDecorator(typeof (LogHandler<, >), typeof (IRequestHandler<, >), "requestHandler", "decoratedWithLog");
            builder.RegisterGenericDecorator(typeof (TransactionHandler<, >), typeof (IRequestHandler<, >), "decoratedWithLog"); //double logged! ->key-less
            var container = builder.Build();

            //get the original undecorated handler by using key
            var commandUndec = container.ResolveKeyed<IRequestHandler<TestRequest, Unit>>("requestHandler");
            commandUndec.Handle(new TestRequest{Title = "undecorated"});
            Console.WriteLine();

            //get the decorated one
            var command = container.Resolve<IRequestHandler<TestRequest, Unit>>();
            command.Handle(new TestRequest{Title = "decorated"});
            Console.WriteLine();
            
            //finally, use the mediator to resolve everything for us
            var _mediator = new Mediator(container);
            _mediator.Send(new TestRequest{Title = "Test"});
            Console.ReadLine();
        }
    }
}