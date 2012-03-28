using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace PostSharp.Toolkit.Tests
{
    public class BaseTestsFixture
    {
        public StringWriter TextWriter { get; private set; }

        public StringBuilder OutputString { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            this.OutputString = new StringBuilder();
            this.TextWriter = new StringWriter(this.OutputString);
            Console.SetOut(this.TextWriter);
            Trace.Listeners.Add(new TextWriterTraceListener(TextWriter));
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (this.TextWriter != null)
            {
                this.TextWriter.Dispose();
            }
        }
    }
}