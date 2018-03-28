using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyInfoReader
{
    public static class EnvironmentHelper
    {
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/z46c489x(v=vs.110).aspx
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="target"></param>
        public static void SetEnvironment(string key, string value, EnvironmentVariableTarget target)
        {
            Environment.SetEnvironmentVariable(key, value, target);
        }
    }
}