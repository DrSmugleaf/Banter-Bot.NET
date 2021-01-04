using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace BanterBot.NET
{
    public class DebugTools
    {
        [Conditional("DEBUG")]
        [AssertionMethod]
        public static void AssertNotNull([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] object? arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException();
            }
        }
    }
}
