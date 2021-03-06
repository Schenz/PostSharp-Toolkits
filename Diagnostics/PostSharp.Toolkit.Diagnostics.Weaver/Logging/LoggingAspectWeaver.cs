﻿using System;
using PostSharp.Extensibility;
using PostSharp.Sdk.AspectWeaver;
using PostSharp.Sdk.AspectWeaver.AspectWeavers;

namespace PostSharp.Toolkit.Diagnostics.Weaver.Logging
{
    internal sealed class LoggingAspectWeaver : MethodLevelAspectWeaver
    {
        private static readonly LogAspectConfigurationAttribute defaultConfiguration = new LogAspectConfigurationAttribute
        {
            OnEntryOptions = LogOptions.IncludeParameterType | LogOptions.IncludeParameterName | LogOptions.IncludeParameterValue, 
            OnExceptionOptions = LogOptions.None,
            OnSuccessOptions = LogOptions.IncludeParameterType | LogOptions.IncludeReturnValue,
            OnEntryLevel = LogLevel.Debug,
            OnSuccessLevel = LogLevel.Debug,
            OnExceptionLevel = LogLevel.Warning
        };

        private LoggingAspectTransformation transformation;

        private InstrumentationPlugIn instrumentationPlugIn;

        public LoggingAspectWeaver()
            : base(defaultConfiguration, MulticastTargets.Property | MulticastTargets.Method | MulticastTargets.Class)
        {
            this.RequiresRuntimeInstance = false;
            this.RequiresRuntimeReflectionObject = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.instrumentationPlugIn = (InstrumentationPlugIn)this.AspectWeaverTask.Project.Tasks[InstrumentationPlugIn.Name];
            this.transformation = new LoggingAspectTransformation(this, this.instrumentationPlugIn.Backend);

            ApplyWaivedEffects(this.transformation);
        }

        protected override AspectWeaverInstance CreateAspectWeaverInstance(AspectInstanceInfo aspectInstanceInfo)
        {
            return new LoggingAspectWeaverInstance(this, aspectInstanceInfo);
        }

        private class LoggingAspectWeaverInstance : MethodLevelAspectWeaverInstance
        {
            public LoggingAspectWeaverInstance(MethodLevelAspectWeaver aspectWeaver, AspectInstanceInfo aspectInstanceInfo)
                : base(aspectWeaver, aspectInstanceInfo)
            {
            }

            public override void ProvideAspectTransformations(AspectWeaverTransformationAdder adder)
            {
                LoggingAspectTransformation transformation = ((LoggingAspectWeaver)AspectWeaver).transformation;
                AspectWeaverTransformationInstance transformationInstance = transformation.CreateInstance(this);

                adder.Add(TargetElement, transformationInstance);
            }
        }

    }
}