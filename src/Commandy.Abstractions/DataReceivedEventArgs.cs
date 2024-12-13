using System;

namespace Commandy.Abstractions
{
    public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);
    public class DataReceivedEventArgs : EventArgs
    {
        public string Data { get; }
        public DataReceivedEventArgs(string data)
        {
            Data = data;
        }
    }
}
