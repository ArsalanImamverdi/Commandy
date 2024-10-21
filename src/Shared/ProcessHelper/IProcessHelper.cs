using System.Diagnostics;
using System.IO;
using System.Threading;

using Commandy.Abstractions;

namespace Commandy.Internals.ProcessHelper
{
    internal interface IProcessHelper
    {
        Process Create(ICommand command, DataReceivedEventHandler onDataReceived, DataReceivedEventHandler onErrorReceived, CancellationToken cancellationToken, StreamReader streamReader);
    }
}
