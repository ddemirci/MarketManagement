using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketManagement.Logger
{
    public class FileLogProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string category)
        {
            return new FileLogger();
        }
        public void Dispose()
        {

        }
    }
}
