using PostSharp.Sdk.CodeModel;

namespace PostSharp.Toolkit.Diagnostics.Weaver.NLog.Logging
{
    internal class LoggerMethods
    {
        public IMethod IsLoggingEnabledMethod { get; private set; }
        public IMethod WriteStringMethod { get; private set; }
        public IMethod WriteStringFormat1Method { get; private set; }
        public IMethod WriteStringFormat2Method { get; private set; }
        public IMethod WriteStringFormat3Method { get; private set; }
        public IMethod WriteStringFormatArrayMethod { get; private set; }
        public IMethod WriteStringExceptionMethod { get; private set; }

        public LoggerMethods(IMethod isLoggingEnabledMethod, IMethod writeStringMethod, IMethod writeStringFormat1Method,
                             IMethod writeStringFormat2Method, IMethod writeStringFormat3Method,
                             IMethod writeStringFormatArrayMethod, IMethod writeStringExceptionMethod)
        {
            this.IsLoggingEnabledMethod = isLoggingEnabledMethod;
            this.WriteStringMethod = writeStringMethod;
            this.WriteStringFormat1Method = writeStringFormat1Method;
            this.WriteStringFormat2Method = writeStringFormat2Method;
            this.WriteStringFormat3Method = writeStringFormat3Method;
            this.WriteStringFormatArrayMethod = writeStringFormatArrayMethod;
            this.WriteStringExceptionMethod = writeStringExceptionMethod;
        }
    }
}