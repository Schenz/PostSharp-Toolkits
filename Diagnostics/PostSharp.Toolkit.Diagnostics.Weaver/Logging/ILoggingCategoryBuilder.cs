﻿using System;
using PostSharp.Sdk.CodeModel;

namespace PostSharp.Toolkit.Diagnostics.Weaver.Logging
{
    public interface ILoggingCategoryBuilder
    {
        bool SupportsIsEnabled { get; }

        void EmitGetIsEnabled(InstructionWriter writer, LogSeverity logSeverity);

        void EmitWrite(InstructionWriter writer, string messageFormattingString, int argumentsCount,
                       LogSeverity logSeverity, Action<InstructionWriter> getExceptionAction,
                       Action<int, InstructionWriter> loadArgumentAction, bool useWrapper);
    }
}