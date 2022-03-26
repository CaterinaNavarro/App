using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreApp.Crosscutting.Exceptions
{
    public static class ExceptionExtension
    {
        public static string Thrower(this Exception @this)
        {
            MethodBase methodBase = new StackTrace(@this, true).GetFrame(0).GetMethod();
            return $"{methodBase.DeclaringType} . {methodBase.Name}";
        }
    }
}
