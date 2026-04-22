using Microsoft.Extensions.DependencyInjection;

namespace Api.Common.Commands
{
    public class MemoryCommandBus : ICommandBus
    {
        private readonly IServiceScopeFactory _scopeFactory;

        // 1. 注入 IServiceScopeFactory
        public MemoryCommandBus(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public TResult Submit<TResult>(ICommand<TResult> command)
        {
            // 2. 创建一个新的依赖注入作用域
            using (var scope = _scopeFactory.CreateScope())
            {
                var handlerType =
                   typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
                
                // 3. 从新创建的作用域中解析处理器
                dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);

                return handler.Handle((dynamic)command);
            }
        }
    }
}
