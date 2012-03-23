using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace PostSharp.Toolkit.Diagnostics
{
    [RequirePostSharp("PostSharp.Toolkit.Diagnostics.Weaver", "PostSharp.Toolkit.Diagnostics")]
    public interface ILogAspect : IAspect
    {
    }
}