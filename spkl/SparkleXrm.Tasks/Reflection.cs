using SparkleXrm.Tasks.Config;
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

        public static Assembly ReflectionOnlyLoadAssembly(string path)
        {
            string[] ignore = new string[] { "Microsoft.Crm.Sdk.Proxy.dll", "Microsoft.IdentityModel.dll", "Microsoft.Xrm.Sdk.dll","Microsoft.Xrm.Sdk.Workflow.dll" };
            if (ignore.Where(a => path.Contains(a)).FirstOrDefault() != null)
                return null;

            Assembly assembly = null;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomain_ReflectionOnlyAssemblyResolve;
            try
            {
                assembly = Assembly.ReflectionOnlyLoadFrom(path);
            }
            catch (FileLoadException ex)
            {
                // Assembly already loaded so skip
                Debug.WriteLine("Assembly load error:" + ex.Message);

            }
            finally
            {
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= CurrentDomain_ReflectionOnlyAssemblyResolve;
            }
            return assembly;
        }

        private static Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly;
            string[] parts = args.Name.Split(',');
            switch (parts[0])
            {
                case "Microsoft.Xrm.Sdk":
                    assembly = System.Reflection.Assembly.ReflectionOnlyLoad(parts[0].Trim());
                    break;
                default:
                    assembly = System.Reflection.Assembly.ReflectionOnlyLoad(args.Name);
                    break;
            }
           
            return assembly;
        }

        public static IEnumerable<Type> GetTypesImplementingInterface(Assembly assembly, Type interfaceName)
        {
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomain_ReflectionOnlyAssemblyResolve;
            var types = assembly.DefinedTypes.Where(p => p.GetInterfaces().FirstOrDefault(a => a.Name == interfaceName.Name) != null);
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= CurrentDomain_ReflectionOnlyAssemblyResolve;
            return types;
        }

        public static IEnumerable<Type> GetTypesInheritingFrom(Assembly assembly, Type type)
        {
            //Load the System.Activities dll to Reflection context so that we can find all types deriving from System.Activities.CodeActivity
            System.Reflection.Assembly.ReflectionOnlyLoad(type.Assembly.FullName);
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomain_ReflectionOnlyAssemblyResolve;
            var systemActivitiesAssemlyLoadedinReflection = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().Where(ab => ab.GetType(type.FullName) != null).First();
            var types = assembly.DefinedTypes.Where(p => systemActivitiesAssemlyLoadedinReflection.GetType(type.FullName).IsAssignableFrom(p));
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= CurrentDomain_ReflectionOnlyAssemblyResolve;
            return types;
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
