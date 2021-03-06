using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Extensibility;

namespace PostSharp.Toolkit.Diagnostics
{
    [Serializable]
    [AttributeUsage(
      AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Module | AttributeTargets.Struct,
      AllowMultiple = true,
      Inherited = false)]
    [MulticastAttributeUsage(
    MulticastTargets.InstanceConstructor | MulticastTargets.StaticConstructor | MulticastTargets.Method,
      AllowMultiple = true)]
    [Metric("UsedFeatures", "Toolkit.Diagnostics.Logging")]
    [AspectConfigurationAttributeType(typeof(LogAspectConfigurationAttribute))]
    public class LogAttribute : MethodLevelAspect, ILogAspect
    {
#if !SMALL
        private LogOptions? onEntryOptions;
        public LogOptions OnEntryOptions
        {
            get { return this.onEntryOptions.GetValueOrDefault(LogOptions.None); }
            set { this.onEntryOptions = value; }
        }

        private LogOptions? onSuccessOptions;
        public LogOptions OnSuccessOptions
        {
            get { return this.onSuccessOptions.GetValueOrDefault(LogOptions.None); }
            set { this.onSuccessOptions = value; }
        }

        private LogOptions? onExceptionOptions;
        public LogOptions OnExceptionOptions
        {
            get { return this.onExceptionOptions.GetValueOrDefault(LogOptions.None); }
            set { this.onExceptionOptions = value; }
        }

        private LogLevel? onEntryLevel;
        public LogLevel OnEntryLevel 
        {
            get { return this.onEntryLevel.GetValueOrDefault(LogLevel.None); }
            set { this.onEntryLevel = value; }
        }

        private LogLevel? onSuccessLevel;
        public LogLevel OnSuccessLevel
        {
            get { return this.onSuccessLevel.GetValueOrDefault(LogLevel.None); }
            set { this.onSuccessLevel = value; }
        }

        private LogLevel? onExceptionLevel;
        public LogLevel OnExceptionLevel
        {
            get { return this.onExceptionLevel.GetValueOrDefault(LogLevel.None); }
            set { this.onExceptionLevel = value; }
        }

        protected override AspectConfiguration CreateAspectConfiguration()
        {
            return new LogAspectConfiguration();
        }

        protected override void SetAspectConfiguration(AspectConfiguration aspectConfiguration, System.Reflection.MethodBase targetMethod)
        {
            LogAspectConfiguration configuration = (LogAspectConfiguration)aspectConfiguration;
            configuration.OnEntryOptions = this.onEntryOptions;
            configuration.OnSuccessOptions = this.onSuccessOptions;
            configuration.OnExceptionOptions = this.onExceptionOptions;
            configuration.OnEntryLevel = this.onEntryLevel;
            configuration.OnSuccessLevel = this.onSuccessLevel;
            configuration.OnExceptionLevel = this.onExceptionLevel;
        }
#endif
    }

  
}