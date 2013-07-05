// DefaultFormattor.cs
//

using Slick;

namespace SparkleXrm.GridEditor
{
    public static class Formatters
    {
        public static string DefaultFormatter(int row, int cell, object value, Column columnDef, object dataContext)
        {

            if (value == null)
            {
                return "";
            }
            else
            {
                return value.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
            }

        }
    }
}
