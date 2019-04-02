namespace SparkleXrm.Tasks.CrmSvcUtil
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class SourceCodeTypeExtractor
    {
        private const string ClassPattern = @"([a-zA-Z0-9\(\"",\s\.\)\]\s\n\[::\.,_])+public\spartial[a-zA-Z0-9\s:\.,_]+{(?:[^{}]|(?<open>{)|(?<-open>}))+(?(open)(?!))}";
        private const string EnumPattern = @"([a-zA-Z0-9\(\"",\s\.\)\]\s\n\[::\.,_])+public\senum[a-zA-Z0-9\s_]+{(?:[^{}]|(?<open>{)|(?<-open>}))+(?(open)(?!))}";

        public List<TypeContainer> ExtractTypes(string input)
        {  
            var classMatches = new Regex(ClassPattern).Matches(input);
            var result = (from Match match in classMatches
                          select new TypeContainer(ContainerType.ClassContainer, match.Value))
                          .ToList();

            var enumMatches = new Regex(EnumPattern).Matches(input);
            result.AddRange(from Match match in enumMatches
                            select new TypeContainer(ContainerType.EnumContainer, match.Value));

            return result;
        }
    }
}