using System;

namespace Iteco.Autotests.Common.Utilities
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestCaseId : Attribute
    {
        public int Id { get; private set; }

        public TestCaseId(int id)
        {
            Id = id;
        }
    }
}