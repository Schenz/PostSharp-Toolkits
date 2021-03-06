﻿using System;
using System.Text;
using PostSharp.Sdk.AspectInfrastructure;
using PostSharp.Sdk.CodeModel;
using PostSharp.Sdk.CodeModel.TypeSignatures;

namespace PostSharp.Toolkit.Diagnostics.Weaver.Logging
{
    internal sealed class MessageArgumentsFormatter
    {
        public const int ThisArgumentPosition = -1;
        public const int ReturnParameterPosition = -2;

        private readonly MethodBodyTransformationContext context;
        private readonly MethodDefDeclaration targetMethod;

        public MessageArgumentsFormatter(MethodBodyTransformationContext context)
        {
            this.context = context;
            this.targetMethod = context.TargetElement as MethodDefDeclaration;
            if (this.targetMethod == null)
            {
                throw new InvalidOperationException("Target element is not a method");
            }
        }

        public string CreateMessageArguments(LogOptions logOptions, out int[] argumentsIndex)
        {
            StringBuilder formatBuilder = new StringBuilder();

            int arrayLength = this.GetArrayLength(logOptions);
            argumentsIndex = new int[arrayLength];
            
            formatBuilder.AppendFormat("{0}.{1}", this.targetMethod.DeclaringType, this.targetMethod.Name);
            formatBuilder.Append("(");

            int startParameter = 0;
            if ((logOptions & LogOptions.IncludeThisArgument) != 0 &&
                (this.context.MethodMapping.MethodSignature.CallingConvention & CallingConvention.HasThis) != 0)
            {
                formatBuilder.Append("this = ");
                AppendFormatPlaceholder(0, formatBuilder, this.targetMethod.DeclaringType);
                startParameter = 1;
                argumentsIndex[0] = ThisArgumentPosition;
            }


            bool includeParameterName = (logOptions & LogOptions.IncludeParameterName) != 0;
            bool includeParameterType = (logOptions & LogOptions.IncludeParameterType) != 0;
            bool includeParameterValue = (logOptions & LogOptions.IncludeParameterValue) != 0;
            
            if (includeParameterName || includeParameterType || includeParameterValue)
            {
                this.WriteMethodArguments(formatBuilder, argumentsIndex, startParameter, includeParameterName, includeParameterType, includeParameterValue);
            }

            formatBuilder.Append(")");

            if (ShouldLogReturnType(logOptions))
            {
                formatBuilder.Append(" : ");
                AppendFormatPlaceholder(argumentsIndex.Length - 1, formatBuilder, this.targetMethod.ReturnParameter.ParameterType);
                argumentsIndex[argumentsIndex.Length - 1] = ReturnParameterPosition;
            }

            return formatBuilder.ToString();

        }

        private void WriteMethodArguments(StringBuilder formatBuilder, int[] argumentsIndex, int startParameter, bool includeParameterName, bool includeParameterType, bool includeParameterValue)
        {
            int parameterCount = this.context.MethodMapping.MethodSignature.ParameterCount;
            for (int i = 0; i < parameterCount; i++)
            {
                int index = i + startParameter;

                if (index > 0)
                {
                    formatBuilder.Append(", ");
                }

                ITypeSignature parameterType = this.context.MethodMapping.MethodSignature.GetParameterType(i);
                if (includeParameterType)
                {
                    formatBuilder.Append(parameterType.GetReflectionName());
                }

                if (includeParameterName)
                {
                    formatBuilder.AppendFormat(" {0}", this.context.MethodMapping.MethodMappingInformation.GetParameterName(i));
                }

                if (includeParameterValue)
                {
                    if (includeParameterName || includeParameterType)
                    {
                        formatBuilder.AppendFormat(" = ");
                    }

                    AppendFormatPlaceholder(index, formatBuilder, parameterType);
                    argumentsIndex[index] = i;
                }
            }
        }

        private int GetArrayLength(LogOptions logOptions)
        {
            int result = 0;

            if ((logOptions & LogOptions.IncludeParameterValue) != 0)
            {
                result = this.context.MethodMapping.MethodSignature.ParameterCount;
            }

            if ((logOptions & LogOptions.IncludeThisArgument) != 0 &&
                (this.context.MethodMapping.MethodSignature.CallingConvention & CallingConvention.HasThis) != 0)
            {
                ++result;
            }

            if (ShouldLogReturnType(logOptions))
            {
                ++result;
            }

            return result;
        }

        private bool ShouldLogReturnType(LogOptions logOptions)
        {
            return (logOptions & LogOptions.IncludeReturnValue) != 0 &&
                   !IntrinsicTypeSignature.Is(this.context.MethodMapping.MethodSignature.ReturnType, IntrinsicType.Void);
        }

        private static void AppendFormatPlaceholder(int i, StringBuilder formatBuilder, ITypeSignature parameterType)
        {
            if (IntrinsicTypeSignature.Is(parameterType, IntrinsicType.String))
            {
                formatBuilder.AppendFormat("\"" + "{{{0}}}" + "\"", i);
            }
            else if (!parameterType.BelongsToClassification(TypeClassifications.Intrinsic) ||
                     IntrinsicTypeSignature.Is(parameterType, IntrinsicType.Object))
            {
                formatBuilder.Append("{{");
                formatBuilder.AppendFormat("{{{0}}}", i);
                formatBuilder.Append("}}");
            }
            else
            {
                formatBuilder.AppendFormat("{{{0}}}", i);
            }
        }
    }
}