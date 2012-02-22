﻿using System;
using PostSharp.Sdk.CodeModel;
using PostSharp.Toolkit.Diagnostics.Weaver.Logging;
using PostSharp.Toolkit.Diagnostics.Weaver.Logging.Context;

namespace PostSharp.Toolkit.Diagnostics.Weaver.NLog.Logging
{
    internal sealed class NLogBackend : LoggingContextBackend
    {
        private ITypeSignature loggerType;
        private ITypeSignature logManagerType;
        private IMethod initializeLoggerMethod;
        
        protected override LoggingContext CreateContext(ModuleDeclaration module)
        {
            return new NLogContextBuilder(module).CreateContext();
        }
    }
}