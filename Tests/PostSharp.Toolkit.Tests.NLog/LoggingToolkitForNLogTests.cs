﻿using System;
using NLog;
using NUnit.Framework;
using PostSharp.Toolkit.Diagnostics;
using TestAssembly;

namespace PostSharp.Toolkit.Tests.NLog
{
    [TestFixture]
    public class LoggingToolkitForNLogTests : ConsoleTestsFixture
    {
        [Test]
        public void NLog_Methods_LogsMethodEnter()
        {
            SimpleClass s = new SimpleClass();
            s.Method1();

            string output = OutputString.ToString();
            StringAssert.Contains("TRACE|TestAssembly.SimpleClass|Entering: TestAssembly.SimpleClass.Method1()", output);
        }

        [Test]
        public void NLog_Properties_LogsPropertyGetter()
        {
            SimpleClass s = new SimpleClass();
            string value = s.Property1;

            string output = OutputString.ToString();
            StringAssert.Contains("TRACE|TestAssembly.SimpleClass|Entering: TestAssembly.SimpleClass.get_Property1()", output);
        }

        [Test]
        public void NLog_Properties_LogsPropertySetter()
        {
            SimpleClass s = new SimpleClass();
            s.Property1 = "Test";

            string output = OutputString.ToString();
            StringAssert.Contains(
                "TRACE|TestAssembly.SimpleClass|Entering: TestAssembly.SimpleClass.set_Property1(string value = \"Test\")", output);
        }

        [Test]
        public void NLog_OnException_PrintsException()
        {
            SimpleClass s = new SimpleClass();
            try
            {
                s.MethodThrowsException();
            }
            catch
            {
            }

            string output = OutputString.ToString();
            StringAssert.Contains("System.Exception", output);
        }

        [Test]
        public void NLog_UserDefinedType_DoesNotLogMethodCallsRecursively()
        {
            Person person = new Person
            {
                FirstName = "John",
                LastName = "Smith"
            };

            string s = person.ToString();
            string output = OutputString.ToString();
            StringAssert.Contains("PostSharp.Toolkit.Tests.NLog.Person|Entering: PostSharp.Toolkit.Tests.NLog.Person.GetFirstName(PostSharp.Toolkit.Tests.NLog.Person person = John Smith)", output);
        }
    }
}
