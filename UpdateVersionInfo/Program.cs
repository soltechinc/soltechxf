using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace UpdateVersionInfo
{
    internal class Program
    {
        static String AssemblyVersionExpression = @"^\s*\[assembly:\s*(?<attribute>(?:System\.)?(?:Reflection\.)?AssemblyVersion(?:Attribute)?\s*\(\s*""(?<version>[^""]+)""\s*\)\s*)\s*\]\s*$";
        static String AssemblyFileVersionExpression = @"^\s*\[assembly:\s*(?<attribute>(?:System\.)?(?:Reflection\.)?AssemblyFileVersion(?:Attribute)?\s*\(\s*""(?<version>[^""]+)""\s*\)\s*)\s*\]\s*$";

        static readonly Regex assemblyVersionRegEx = new Regex(AssemblyVersionExpression, RegexOptions.Multiline | RegexOptions.Compiled);
        static readonly Regex assemblyFileVersionRegEx = new Regex(AssemblyFileVersionExpression, RegexOptions.Multiline | RegexOptions.Compiled);

        static void Main(string[] args)
        {
            var commandLine = new CommandLineArguments(args);
            try
            {
                ValidateCommandLine(commandLine);

                Version version = new Version(
                    commandLine.Major, 
                    commandLine.Minor, 
                    commandLine.Build.Value, 
                    commandLine.Revision.HasValue ? commandLine.Revision.Value : 0);

                UpdateCSVersionInfo(commandLine.VersionCsPath, version);
                if (!String.IsNullOrEmpty(commandLine.AndroidManifestPath))
                {
                    UpdateAndroidVersionInfo(commandLine.AndroidManifestPath, version);
                }
                if (!String.IsNullOrEmpty(commandLine.TouchPListPath))
                {
                    UpdateTouchVersionInfo(commandLine.TouchPListPath, version);
                }

            }
            catch (Exception e)
            {
                WriteHelp(commandLine, "An unexpected error was encountered:" + e.Message);
            }
        }

        private static void UpdateCSVersionInfo(string path, Version version)
        {
            String contents;
            using (var reader = new StreamReader(path))
            {
                contents = reader.ReadToEnd();
            }
            contents = assemblyVersionRegEx.Replace(contents, "[assembly: System.Reflection.AssemblyVersion(\"" + version.ToString() + "\")]");
            if (assemblyFileVersionRegEx.IsMatch(contents))
            {
                contents = assemblyFileVersionRegEx.Replace(contents, "[assembly: System.Reflection.AssemblyFileVersion(\"" + version.ToString() + "\")]");
            }
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(contents);
            }
        }

        private static void UpdateAndroidVersionInfo(string path, Version version)
        {
            const string androidNS = "http://schemas.android.com/apk/res/android";
            XName versionCodeAttributeName = XName.Get("versionCode", androidNS);
            XName versionNameAttributeName = XName.Get("versionName", androidNS);
            XDocument doc = XDocument.Load(path);
            doc.Root.SetAttributeValue(versionCodeAttributeName, version.Build);
            doc.Root.SetAttributeValue(versionNameAttributeName, version);
            doc.Save(path);
        }

        private static void UpdateTouchVersionInfo(string path, Version version)
        {
            XDocument doc = XDocument.Load(path);
            var shortVersionElement = doc.XPathSelectElement("plist/dict/key[string()='CFBundleShortVersionString']");
            var versionElement = shortVersionElement.NextNode as XElement;
            versionElement.Value = version.ToString();
            doc.Save(path);
        }


        private static void ValidateCommandLine(CommandLineArguments commandLine)
        {
            if (commandLine.ShowHelp)
            {
                WriteHelp(commandLine);
                return;
            }
            var errors = new System.Text.StringBuilder();
            if (commandLine.Major < 0)
            {
                errors.AppendLine("You must supply a positive major version number.");
            }
            if (commandLine.Minor < 0)
            {
                errors.AppendLine("You must supply a positive minor version number.");
            }
            if (!commandLine.Build.HasValue)
            {
                errors.AppendLine("You must supply a numeric build number.");
            }
            if (String.IsNullOrEmpty(commandLine.VersionCsPath) || !IsValidCSharpVersionFile(commandLine.VersionCsPath))
            {
                errors.AppendLine("You must supply valid path to a writable C# file containing assembly version information.");
            }
            if (!String.IsNullOrEmpty(commandLine.AndroidManifestPath) && !IsValidAndroidManifest(commandLine.AndroidManifestPath))
            {
                errors.AppendLine("You must supply valid path to a writable android manifest file.");
            }
            if (!String.IsNullOrEmpty(commandLine.TouchPListPath) && !IsValidTouchPList(commandLine.TouchPListPath))
            {
                errors.AppendLine("You must supply valid path to a writable plist file containing version information.");
            }
            if (errors.Length > 0)
            {
                WriteHelp(commandLine, "Invalid command line:\n" + errors.ToString());
            }
        }

        private static bool IsValidCSharpVersionFile(String path)
        {
            if (!File.Exists(path)) return false;
            if ((new FileInfo(path).Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) return false;

            try
            {
                String contents;
                using (var reader = new StreamReader(path))
                {
                    contents = reader.ReadToEnd();
                }

                if (assemblyVersionRegEx.IsMatch(contents))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError(e.Message);
            }
            
            return false;
        }

        private static bool IsValidAndroidManifest(String path)
        {
            if (!File.Exists(path)) return false;
            if ((new FileInfo(path).Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) return false;

            try
            {
                // <manifest ...
                XDocument doc = XDocument.Load(path);
                var rootElement = doc.Root as XElement;
                if (rootElement != null && rootElement.Name == "manifest") return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError(e.Message);
            }
            return false;
        }

        private static bool IsValidTouchPList(String path)
        {
            if (!File.Exists(path)) return false;
            if ((new FileInfo(path).Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) return false;

            try
            {
                //<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
                XDocument doc = XDocument.Load(path);
                if (doc.DocumentType.Name == "plist")
                {
                    var shortVersionElement = doc.XPathSelectElement("plist/dict/key[string()='CFBundleShortVersionString']");
                    if (shortVersionElement != null)
                    {
                        var valueElement = shortVersionElement.NextNode as XElement;
                        if (valueElement != null && valueElement.Name == "string") return true;
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError(e.Message);
            }
            
            return false;
        }

        private static void WriteHelp(CommandLineArguments commandLine, String message = null)
        {
            if (!String.IsNullOrEmpty(message))
            {
                Console.WriteLine(message);
            }
            commandLine.WriteHelp(Console.Out);
        }
    }
}
