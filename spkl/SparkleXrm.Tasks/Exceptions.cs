using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleXrm.Tasks
{
    public class SparkleTaskException : Exception
    {
        public enum ExceptionTypes {
           DUPLICATE_STEP,
            DUPLICATE_FILE,
            CONFIG_NOTFOUND,
            NO_TASK_SUPPLIED,
            AUTH_ERROR,
            UTILSNOTFOUND
        }

        public ExceptionTypes ExceptionType { get; protected set; }
        public SparkleTaskException(ExceptionTypes exectionType, string message) :base (message)
        {
            ExceptionType = exectionType;
        }
    }
}
