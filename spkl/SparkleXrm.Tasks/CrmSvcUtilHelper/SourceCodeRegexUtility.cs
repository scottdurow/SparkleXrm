using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SparkleXrm.Tasks.CrmSvcUtilHelper
{
    public class SourceCodeRegexUtility
    {
        private const string ClassPattern = @"([a-zA-Z0-9\(\"",\s\.\)\]\s\n\[:])+public\spartial[a-zA-Z0-9\s:\.,_]+{(?:[^{}]|(?<open>{)|(?<-open>}))+(?(open)(?!))}";
        private const string EnumPattern = @"([a-zA-Z0-9\(\"",\s\.\)\]\s\n\[:])+public\senum[a-zA-Z0-9\s_]+{(?:[^{}]|(?<open>{)|(?<-open>}))+(?(open)(?!))}";

        public List<TypeContainer> ExtractTypes(string input)
        {
            var result = new List<TypeContainer>();

            var regex = new Regex(ClassPattern);
            var matches = regex.Matches(input);
            foreach (Match match in matches)
            {
                // Console.WriteLine("\n**********************");
                // Console.WriteLine(match.Value);
                //  Console.WriteLine("**********************\n\n");
                result.Add(new TypeContainer(ContainerType.ClassContainer, match.Value));
            }

            regex = new Regex(EnumPattern);
            var matches2 = regex.Matches(input);
            foreach (Match match in matches2)
            {
                // Console.WriteLine("\n**********************");
                //  Console.WriteLine(match.Value);
                //  Console.WriteLine("**********************\n\n");
                result.Add(new TypeContainer(ContainerType.EnumContainer, match.Value));
            }

            return result;
        }
    }
}