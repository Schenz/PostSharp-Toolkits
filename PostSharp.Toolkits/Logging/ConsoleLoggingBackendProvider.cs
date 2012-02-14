using System;

namespace PostSharp.Toolkit.Instrumentation.Weaver.Logging
{
    internal sealed class ConsoleLoggingBackendProvider : ILoggingBackendProvider
    {
        public ILoggingBackend GetBackend(string name)
        {
            if (name.Equals("console", StringComparison.OrdinalIgnoreCase))
                return new ConsoleLoggingBackend();

            return null;
        }
    }
}