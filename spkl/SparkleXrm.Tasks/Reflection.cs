﻿using SparkleXrm.Tasks.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SparkleXrm.Tasks
{
    public class Reflection
    {
        public static string[] IgnoredAssemblies = new string[] {
            "Microsoft.Crm.Sdk.Proxy.dll",
            "Microsoft.IdentityModel.dll",
            "Microsoft.Xrm.Sdk.dll",
            "Microsoft.Xrm.Sdk.Workflow.dll",
            "Microsoft.IdentityModel.Clients.ActiveDirectory.dll",
            "Microsoft.Extensions.FileSystemGlobbing.dll",
            "Microsoft.IdentityModel.Clients.ActiveDirectory.WindowsForms.dll",
            "Microsoft.Xrm.Sdk.Deployment.dll",
            "Microsoft.Xrm.Tooling.Connector.dll",
            "Newtonsoft.Json.dll",
            "SparkleXrm.Tasks.dll",
            "System.Net.Http.dll",
            "Microsoft.Rest.ClientRuntime.dll"
        };

        public static Assembly LoadAssembly(string path)
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.LoadFrom(path);
            }
            catch (FileLoadException ex)
            {
                // Assembly already loaded so skip
                Debug.WriteLine("Assembly load error:" + ex.Message);
            }
            return assembly;
        }

        public static IEnumerable<Type> GetTypesImplementingInterface(Assembly assembly, Type interfaceName)
        {
            var types = assembly.ExportedTypes.Where(p => p.GetInterfaces().FirstOrDefault(a => a.Name == interfaceName.Name) != null);
            Trace.WriteLine(types.FirstOrDefault()?.CustomAttributes.Count());
            return types;
        }

        public static IEnumerable<Type> GetTypesInheritingFrom(Assembly assembly, Type type)
        {
            var definedTypes = assembly.DefinedTypes.Where(p => p.BaseType != null && p.BaseType.Name == type.Name).ToList();

            var allTypes = new List<TypeInfo>(definedTypes);
            foreach (var abstractType in definedTypes.Where(t => t.IsAbstract))
            {
                var inheritingTypes = assembly.DefinedTypes.Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(abstractType.UnderlyingSystemType)).ToList();
                allTypes.AddRange(inheritingTypes);
            }

            return allTypes;
        }

        public static IEnumerable<CustomAttributeData> GetAttributes(IEnumerable<Type> types, string attributeName)
        {
            List<CustomAttributeData> attributes = new List<CustomAttributeData>();
            foreach (Type type in types)
            {
                var data = type.GetCustomAttributesData().Where(a => a.AttributeType.Name == attributeName);
                // Don't allow multiple steps with the same name per type
                var duplicateNames = data.Select(a => a.CreateFromData()).GroupBy(s => s.Name).SelectMany(grp => grp.Skip(1));
                if (duplicateNames.Count() > 0)
                {
                    var names = string.Join(", ", duplicateNames.Select(a => a.Name).ToArray());
                    throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.DUPLICATE_STEP, string.Format("Found types with duplicate attributes of the same name(s) {0}", names));
                }
                attributes.AddRange(data);
            }

            return attributes;
        }
    }
}