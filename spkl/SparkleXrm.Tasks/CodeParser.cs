using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SparkleXrm.Tasks
{
    /// <summary>
    /// Responsible for parsing plugin/workflow activity classes and adding deployment attribute metadata
    /// </summary>
    public class CodeParser
    {
        #region Private Fields
        private string _filePath;
        private string _code;
        private Encoding _encoding;
        private Dictionary<string,Match> _pluginClasses;
        private Dictionary<string, Match> _pluginTypes;
        private Dictionary<string, Match> _workflowTypes;
        private Dictionary<string, string> _namespaces = new Dictionary<string, string>();
        #endregion

        #region Private Constants
        private string _classRegex = @"((public( sealed)? class (?'class'[\w]*)[\W]*?)((?'plugin':[\W]*?((IPlugin)|(PluginBase)|(Plugin)))|(?'wf':[\W]*?CodeActivity)))";
        private const string _attributeRegex = @"([ ]*?)\[CrmPluginRegistration\(([\W\w\s]+?)(\)\])([ ]*?(\r\n|\r|\n))";
        private const string _namespaceRegEx = @"namespace (?'ns'[\w.]*)";
        #endregion

        #region Constructors
        public CodeParser(Uri filePath) : this(filePath, null) 
        {
        }
        public CodeParser(Uri filePath, string customClassRegex) 
        {
            _filePath = filePath.OriginalString;
            _code = File.ReadAllText(_filePath);

            // Get current encoding of the file
            // Need to read part of the file to get the current encoding
            using (var reader = new StreamReader(_filePath, Encoding.Default, true))
            {
                if (reader.Peek() >= 0)
                {
                    reader.Read();
                }

                _encoding = reader.CurrentEncoding;
            }

            if (customClassRegex != null)
                ClassRegex = customClassRegex;

            Init();
        }
        public CodeParser(string code) : this(code, null)
        {  
        }
        public CodeParser(string code, string customClassRegex)
        {
            _code = code;
            if (customClassRegex != null)
                ClassRegex = customClassRegex;
            Init();
        }

        private void Init()
        {
            var classMatches = Regex.Matches(_code, _classRegex).Cast<Match>().Where(m => m.Groups.Count > 3).ToArray();
            var classes = classMatches.ToDictionary(delegate (Match match)
            {
                return match.Groups["class"].Value;
            });

            var namespaces = Regex.Matches(_code, _namespaceRegEx).Cast<Match>().Reverse().ToDictionary(delegate (Match match)
            {
                return match.Index;
            });

            _pluginClasses = new Dictionary<string, Match>();
            foreach (var match in classes)
            {
                // Find the namespace before the position
                var namespaceMatch = namespaces.Values.FirstOrDefault(n => n.Index <= match.Value.Index);
                if (namespaceMatch == null)
                    throw new Exception(String.Format("Cannot find namespace for class {0}", match.Value));

                _namespaces[match.Key] = namespaceMatch.Groups["ns"].Value;
                _pluginClasses[namespaceMatch.Groups["ns"].Value + "." + match.Key] = classes[match.Key];
            }

            _pluginTypes = classMatches.Where(a => a.Groups["plugin"].Length > 0).ToDictionary(delegate (Match match)
            {
                var className = match.Groups["class"].Value;
                return _namespaces[className] + "." + className;
            });

            _workflowTypes = classMatches.Where(a => a.Groups["wf"].Length > 0).ToDictionary(delegate (Match match)
            {
                var className = match.Groups["class"].Value;
                return _namespaces[className] + "." + className;
            });
        }
        #endregion

        #region Properties
        public string Code
        {
            get { return _code; }
        }

        public Encoding CurEncoding
        {
            get { return _encoding; }
        }
        public string FilePath
        {
            get { return _filePath; }
        }

        public List<string> ClassNames
        {
            get { return _pluginClasses.Keys.ToList(); }
        }

        public int PluginCount
        {
            get { return _pluginClasses.Keys.Count; }
        }
        public string ClassRegex
        {
            get { return _classRegex; }
            set { _classRegex = value; }
        }
        #endregion

        #region Methods
        public bool IsPlugin(string className)
        {
            return _pluginTypes.Keys.Contains(className);
        }

        public bool IsWorkflowActivity(string className)
        {
            return _workflowTypes.Keys.Contains(className);
        }

        public int RemoveExistingAttributes()
        {
            int count = 0;
            MatchEvaluator evaluator = delegate (Match match)
            {
                count++;
                return "" ;
            };
            
            _code = Regex.Replace(_code, _attributeRegex, evaluator);
            return count;
        }



        public void AddAttribute(CrmPluginRegistrationAttribute attribute, string className)
        {
            // Locate the start of the class and add the attribute above
            var classLocation = _pluginClasses.ContainsKey(className) ? _pluginClasses[className] : null;
            if (classLocation == null)
                throw new Exception(String.Format("Cannot find class {0}", className));

            // start index of "public class OpportunityPluign"
            int classStartIndex = _code.IndexOf(classLocation.Value);

            // start index of "{ public class OpportunityPluign"
            int openBraceBeforeClassIndex = _code.LastIndexOf("{", classStartIndex - 1);

            // start index of "<EOL> { public class OpportunityPluign"
            int eolBeforeClassIndex = _code.LastIndexOf("\r\n", classStartIndex - 1, classStartIndex - 1 - openBraceBeforeClassIndex);

            // discover indentation between class and EOL (inclusive of EOL character)
            string indentation = "\r\n";
            if (eolBeforeClassIndex != -1)
            {
                indentation = _code.Substring(eolBeforeClassIndex, classStartIndex - eolBeforeClassIndex);
            }

            // generate the attribut code
            string attributeCode = attribute.GetAttributeCode(indentation);

            if (eolBeforeClassIndex == -1)
            {
                // insert attribute code at class start
                eolBeforeClassIndex = classStartIndex;
                
                // add EOL between attribute code and class
                attributeCode += "\r\n";
            }

            // insert attribute code
            _code = _code.Insert(eolBeforeClassIndex, attributeCode);
        }
        #endregion
    }
}
