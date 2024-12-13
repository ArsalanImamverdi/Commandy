using Commandy.Abstractions;
using Commandy.DependencyInjection.Internals;
using Commandy.Internals.ShellHelper;

using Microsoft.Extensions.DependencyInjection;

namespace Commandy.DependencyInjection
{
    public static class CommandyServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandy(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(ShellHelperFactory.GetShellHelper());
            serviceCollection.AddSingleton<ICommandProvider, CommandProvider>();
            return serviceCollection;
        }
    }
}
