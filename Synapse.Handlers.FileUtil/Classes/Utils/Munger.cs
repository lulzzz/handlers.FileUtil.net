﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.XmlTransform;
using Alphaleonis.Win32.Filesystem;
using io = System.IO;
using System.Xml;
using System.Text.RegularExpressions;

using Zephyr.Filesystem;

namespace Synapse.Handlers.FileUtil
{
    class Munger
    {
        static public void XMLTransform(String sourceFile, String destinationFile, String transformFile, bool overwrite = true, Clients clients = null)
        {
            bool tempFileUsed = false;
            ZephyrFile source = Utilities.GetZephyrFile(sourceFile, clients);
            ZephyrFile destination = Utilities.GetZephyrFile(destinationFile, clients);
            ZephyrFile transform = Utilities.GetZephyrFile(transformFile, clients);

            if (!overwrite)
            {
                if (destination == null)
                    throw new Exception($"Destination File Is Not Provided, and Overwrite Is Set To [{overwrite}].");
                else if (destination.Exists)
                    throw new Exception($"Destination File [{destinationFile}] Already Exists.");
            }

            io.Stream sourceStream = source?.Open(AccessType.Read);
            io.Stream destinationStream = destination?.Open(AccessType.Write);
            io.Stream transformStream = transform?.Open(AccessType.Read);

            if (destinationStream == null)
            {
                String tempFileName = $"{source.FullName}_tmpout";
                destination = Utilities.GetZephyrFile(tempFileName, clients);
                destination.Create(overwrite);
                destinationStream = destination?.Open(AccessType.Write);
                tempFileUsed = true;
            }

            XMLTransform(sourceStream, destinationStream, transformStream);

            if (tempFileUsed)
            {
                sourceStream.Close();
                destinationStream.Close();
                source.Close();
                destination.Close();

                source.Delete();
                destination.MoveTo(source);
            }
        }

        static public void XMLTransform(String sourceFile, String destinationFile, io.Stream transformStream, bool overwrite = true, Clients clients = null)
        {
            bool tempFileUsed = false;
            ZephyrFile source = Utilities.GetZephyrFile(sourceFile, clients);
            ZephyrFile destination = Utilities.GetZephyrFile(destinationFile, clients);

            if (!overwrite)
            {
                if (destination == null)
                    throw new Exception($"Destination File Is Not Provided, and Overwrite Is Set To [{overwrite}].");
                else if (destination.Exists)
                    throw new Exception($"Destination File [{destinationFile}] Already Exists.");
            }

            io.Stream sourceStream = source?.Open(AccessType.Read);
            io.Stream destinationStream = destination?.Open(AccessType.Write);

            if (destinationStream == null)
            {
                String tempFileName = $"{source.FullName}_tmpout";
                destination = Utilities.GetZephyrFile(tempFileName, clients);
                destination.Create(overwrite);
                destinationStream = destination?.Open(AccessType.Write);
                tempFileUsed = true;
            }

            XMLTransform(sourceStream, destinationStream, transformStream);

            if (tempFileUsed)
            {
                sourceStream.Close();
                destinationStream.Close();
                source.Close();
                destination.Close();

                source.Delete();
                destination.MoveTo(source);
            }
        }

        static public void XMLTransform(io.Stream sourceStream, io.Stream destinationStream, io.Stream transformStream)
        {
            using (XmlTransformableDocument doc = new XmlTransformableDocument())
            {
                doc.PreserveWhitespace = true;

                using (io.StreamReader sr = new io.StreamReader(sourceStream))
                {
                    doc.Load(sr);
                }

                using (XmlTransformation xt = new XmlTransformation(transformStream, null))
                {
                    xt.Apply(doc);
                    doc.Save(destinationStream);
                }
            }
        }

        static public void KeyValue(PropertyFile.Type type, String sourceFile, String destinationFile, String transformFile, List<KeyValuePair<String, String>> settings, bool createIfNotFound = false, bool overwrite = true, Clients clients = null)
        {
            bool tempFileUsed = false;
            ZephyrFile source = Utilities.GetZephyrFile(sourceFile, clients);
            ZephyrFile destination = Utilities.GetZephyrFile(destinationFile, clients);
            ZephyrFile transform = Utilities.GetZephyrFile(transformFile, clients);

            if (!overwrite)
            {
                if (destination == null)
                    throw new Exception($"Destination File Is Not Provided, and Overwrite Is Set To [{overwrite}].");
                else if (destination.Exists)
                    throw new Exception($"Destination File [{destinationFile}] Already Exists.");
            }

            io.Stream sourceStream = source?.Open(AccessType.Read);
            io.Stream destinationStream = destination?.Open(AccessType.Write);
            io.Stream transformStream = transform?.Open(AccessType.Read);

            if (destinationStream == null)
            {
                String tempFileName = $"{source.FullName}_tmpout";
                destination = Utilities.GetZephyrFile(tempFileName, clients);
                destination.Create(overwrite);
                destinationStream = destination?.Open(AccessType.Write);
                tempFileUsed = true;
            }

            KeyValue(type, sourceStream, destinationStream, transformStream, settings, createIfNotFound);

            if (tempFileUsed)
            {
                sourceStream.Close();
                destinationStream.Close();
                source.Close();
                destination.Close();

                source.Delete();
                destination.MoveTo(source);
            }
        }

        static public void KeyValue(PropertyFile.Type type, String sourceFile, String destinationFile, io.Stream transformStream, List<KeyValuePair<String, String>> settings, bool createIfNotFound = false, bool overwrite = true, Clients clients = null)
        {
            bool tempFileUsed = false;
            ZephyrFile source = Utilities.GetZephyrFile(sourceFile, clients);
            ZephyrFile destination = Utilities.GetZephyrFile(destinationFile, clients);

            if (!overwrite)
            {
                if (destination == null)
                    throw new Exception($"Destination File Is Not Provided, and Overwrite Is Set To [{overwrite}].");
                else if (destination.Exists)
                    throw new Exception($"Destination File [{destinationFile}] Already Exists.");
            }

            io.Stream sourceStream = source?.Open(AccessType.Read);
            io.Stream destinationStream = destination?.Open(AccessType.Write);

            if (destinationStream == null)
            {
                String tempFileName = $"{source.FullName}_tmpout";
                destination = Utilities.GetZephyrFile(tempFileName, clients);
                destination.Create(overwrite);
                destinationStream = destination?.Open(AccessType.Write);
                tempFileUsed = true;
            }

            KeyValue(type, sourceStream, destinationStream, transformStream, settings, createIfNotFound);

            if (tempFileUsed)
            {
                sourceStream.Close();
                destinationStream.Close();
                source.Close();
                destination.Close();

                source.Delete();
                destination.MoveTo(source);
            }
        }

        static public void KeyValue(PropertyFile.Type type, io.Stream sourceStream, io.Stream destinationStream, io.Stream transformStream, List<KeyValuePair<String, String>> settings, bool createIfNotFound = false)
        {
            PropertyFile props = new PropertyFile(type, sourceStream);

            if (transformStream != null)
            {
                using (io.StreamReader reader = new io.StreamReader(transformStream))
                {
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        char[] delims = { ',' };
                        String[] values = line.Split(delims);

                        String section = null;
                        String key = null;
                        String value = null;

                        if (values.Length == 2)
                        {
                            key = values[0].Trim();
                            value = values[1].Trim();
                        }
                        else if (values.Length >= 3)
                        {
                            section = values[0].Trim();
                            key = values[1].Trim();
                            value = values[2].Trim();
                        }
                        else
                            continue;

                        if (!String.IsNullOrWhiteSpace(section))
                            if (section.StartsWith(@""""))
                                section = section.Substring(1, section.Length - 2);

                        if (!String.IsNullOrWhiteSpace(key))
                            if (key.StartsWith(@""""))
                                key = key.Substring(1, key.Length - 2);

                        if (!String.IsNullOrWhiteSpace(value))
                            if (value.Trim().StartsWith(@""""))
                                value = value.Substring(1, value.Length - 2);

                        if (props.Exists(section, key))
                            props.SetProperty(section, key, value);
                        else if (createIfNotFound)
                            props.AddProperty(section, key, value);

                    }
                }
            }

            if (settings != null)
            {
                foreach (KeyValuePair<String, String> setting in settings)
                {
                    String section = String.Empty;
                    String key = setting.Key;
                    String value = setting.Value;

                    System.Text.RegularExpressions.Match match = Regex.Match(setting.Key, @"^\[(.*?)\]\s*:\s*(.*?)\s*$", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        section = match.Groups[1].Value;
                        key = match.Groups[2].Value;
                    }

                    if (props.Exists(section, key))
                        props.SetProperty(section, key, value);
                    else if (createIfNotFound)
                        props.AddProperty(section, key, value);
                }
            }

            if (destinationStream == null)
                props.Save(sourceStream);
            else
                props.Save(destinationStream);
        }

        static public void XPath(String sourceFile, String destinationFile, String transformFile, List<KeyValuePair<String, String>> settings, bool overwrite = true, Clients clients = null)
        {
            bool tempFileUsed = false;
            ZephyrFile source = Utilities.GetZephyrFile(sourceFile, clients);
            ZephyrFile destination = Utilities.GetZephyrFile(destinationFile, clients);
            ZephyrFile transform = Utilities.GetZephyrFile(transformFile, clients);

            if (!overwrite)
            {
                if (destination == null)
                    throw new Exception($"Destination File Is Not Provided, and Overwrite Is Set To [{overwrite}].");
                else if (destination.Exists)
                    throw new Exception($"Destination File [{destinationFile}] Already Exists.");
            }

            io.Stream sourceStream = source?.Open(AccessType.Read);
            io.Stream destinationStream = destination?.Open(AccessType.Write);
            io.Stream transformStream = transform?.Open(AccessType.Read);

            if (destinationStream == null)
            {
                String tempFileName = $"{source.FullName}_tmpout";
                destination = Utilities.GetZephyrFile(tempFileName, clients);
                destination.Create(overwrite);
                destinationStream = destination?.Open(AccessType.Write);
                tempFileUsed = true;
            }

            XPath(sourceStream, destinationStream, transformStream, settings);

            if (tempFileUsed)
            {
                sourceStream.Close();
                destinationStream.Close();
                source.Close();
                destination.Close();

                source.Delete();
                destination.MoveTo(source);
            }
        }

        static public void XPath(String sourceFile, String destinationFile, io.Stream transformStream, List<KeyValuePair<String, String>> settings, bool overwrite = true, Clients clients = null)
        {
            bool tempFileUsed = false;
            ZephyrFile source = Utilities.GetZephyrFile(sourceFile, clients);
            ZephyrFile destination = Utilities.GetZephyrFile(destinationFile, clients);

            if (!overwrite)
            {
                if (destination == null)
                    throw new Exception($"Destination File Is Not Provided, and Overwrite Is Set To [{overwrite}].");
                else if (destination.Exists)
                    throw new Exception($"Destination File [{destinationFile}] Already Exists.");
            }

            io.Stream sourceStream = source?.Open(AccessType.Read);
            io.Stream destinationStream = destination?.Open(AccessType.Write);

            if (destinationStream == null)
            {
                String tempFileName = $"{source.FullName}_tmpout";
                destination = Utilities.GetZephyrFile(tempFileName, clients);
                destination.Create(overwrite);
                destinationStream = destination?.Open(AccessType.Write);
                tempFileUsed = true;
            }

            XPath(sourceStream, destinationStream, transformStream, settings);

            if (tempFileUsed)
            {
                sourceStream.Close();
                destinationStream.Close();
                source.Close();
                destination.Close();

                source.Delete();
                destination.MoveTo(source);
            }
        }

        static public void XPath(io.Stream sourceStream, io.Stream destinationStream, io.Stream transformStream, List<KeyValuePair<String, String>> settings)
        {
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            doc.Load(sourceStream);

            if (transformStream != null)
            {
                if (transformStream != null)
                {
                    using (io.StreamReader reader = new io.StreamReader(transformStream))
                    {
                        String line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            char[] delims = { ',' };
                            String[] values = line.Split(delims);

                            String key = null;
                            String value = null;

                            if (values.Length >= 2)
                            {
                                key = values[0].Trim();
                                value = values[1].Trim();
                            }
                            else
                                continue;

                            if (!String.IsNullOrWhiteSpace(key))
                                if (key.StartsWith(@""""))
                                    key = key.Substring(1, key.Length - 2);

                            if (!String.IsNullOrWhiteSpace(value))
                                if (value.Trim().StartsWith(@""""))
                                    value = value.Substring(1, value.Length - 2);

                            XmlNodeList nodes = doc.SelectNodes(key);
                            foreach (XmlNode node in nodes)
                                node.InnerText = value;
                        }
                    }
                }
            }
            
            foreach (KeyValuePair<String, String> setting in settings)
            {
                String localValue = "";
                if (setting.Value != null)
                {
                    localValue = setting.Value;
                }

                if (setting.Key != null)
                {
                    XmlNodeList nodes = doc.SelectNodes(setting.Key);
                    foreach (XmlNode node in nodes)
                        node.InnerText = localValue;
                }
            }

            if (destinationStream == null)
                doc.Save(sourceStream);
            else
                doc.Save(destinationStream);
        }

        static public void RegexMatch(String sourceFile, String destinationFile, String transformFile, List<KeyValuePair<String, String>> settings, bool overwrite = true, Clients clients = null)
        {
            bool tempFileUsed = false;
            ZephyrFile source = Utilities.GetZephyrFile(sourceFile, clients);
            ZephyrFile destination = Utilities.GetZephyrFile(destinationFile, clients);
            ZephyrFile transform = Utilities.GetZephyrFile(transformFile, clients);

            if (!overwrite)
            {
                if (destination == null)
                    throw new Exception($"Destination File Is Not Provided, and Overwrite Is Set To [{overwrite}].");
                else if (destination.Exists)
                    throw new Exception($"Destination File [{destinationFile}] Already Exists.");
            }

            io.Stream sourceStream = source?.Open(AccessType.Read);
            io.Stream destinationStream = destination?.Open(AccessType.Write);
            io.Stream transformStream = transform?.Open(AccessType.Read);

            if (destinationStream == null)
            {
                String tempFileName = $"{source.FullName}_tmpout";
                destination = Utilities.GetZephyrFile(tempFileName, clients);
                destination.Create(overwrite);
                destinationStream = destination?.Open(AccessType.Write);
                tempFileUsed = true;
            }

            RegexMatch(sourceStream, destinationStream, transformStream, settings);

            if (tempFileUsed)
            {
                sourceStream.Close();
                destinationStream.Close();
                source.Close();
                destination.Close();

                source.Delete();
                destination.MoveTo(source);
            }
        }

        static public void RegexMatch(String sourceFile, String destinationFile, io.Stream transformStream, List<KeyValuePair<String, String>> settings, bool overwrite = true, Clients clients = null)
        {
            bool tempFileUsed = false;
            ZephyrFile source = Utilities.GetZephyrFile(sourceFile, clients);
            ZephyrFile destination = Utilities.GetZephyrFile(destinationFile, clients);

            if (!overwrite)
            {
                if (destination == null)
                    throw new Exception($"Destination File Is Not Provided, and Overwrite Is Set To [{overwrite}].");
                else if (destination.Exists)
                    throw new Exception($"Destination File [{destinationFile}] Already Exists.");
            }

            io.Stream sourceStream = source?.Open(AccessType.Read);
            io.Stream destinationStream = destination?.Open(AccessType.Write);

            if (destinationStream == null)
            {
                String tempFileName = $"{source.FullName}_tmpout";
                destination = Utilities.GetZephyrFile(tempFileName, clients);
                destination.Create(overwrite);
                destinationStream = destination?.Open(AccessType.Write);
                tempFileUsed = true;
            }

            RegexMatch(sourceStream, destinationStream, transformStream, settings);

            if (tempFileUsed)
            {
                sourceStream.Close();
                destinationStream.Close();
                source.Close();
                destination.Close();

                source.Delete();
                destination.MoveTo(source);
            }
        }

        static public void RegexMatch(io.Stream sourceStream, io.Stream destinationStream, io.Stream transformStream, List<KeyValuePair<String, String>> settings)
        {
            List<String> xformLines = new List<string>();

            if (transformStream != null)
            {
                using (io.StreamReader xformReader = new io.StreamReader(transformStream))
                {
                    String xformLine;
                    while ((xformLine = xformReader.ReadLine()) != null)
                        xformLines.Add(xformLine);
                }
            }

            String line = null;
            io.StreamReader reader = new io.StreamReader(sourceStream);
            io.StreamWriter writer = null;

            if (destinationStream == null)
                writer = new io.StreamWriter(sourceStream);
            else
                writer = new io.StreamWriter(destinationStream);

            while ((line = reader.ReadLine()) != null)
            {
                // Apply Settings From Transform File
                if (xformLines != null)
                {
                    foreach (String xformLine in xformLines)
                    {
                        char[] delims = { ',' };
                        String[] values = xformLine.Split(delims);

                        String key = null;
                        String value = null;

                        if (values.Length >= 2)
                        {
                            key = values[0].Trim();
                            value = values[1].Trim();
                        }
                        else
                            continue;

                        if (Regex.IsMatch(line, key))
                        {
                            line = DoRegexReplaceWithAutoSave(line, key, value, RegexOptions.IgnoreCase);
                        }
                    }
                }

                // Apply Settings From Package
                foreach (KeyValuePair<String, String> setting in settings)
                {
                    if (Regex.IsMatch(line, setting.Key))
                    {
                        if (setting.Value != null)
                        {
                            String localValue = setting.Value;
                            line = DoRegexReplaceWithAutoSave(line, setting.Key, localValue, RegexOptions.IgnoreCase);
                        }
                        else
                            line = DoRegexReplaceWithAutoSave(line, setting.Key, "", RegexOptions.IgnoreCase);
                    }
                }

                writer.WriteLine(line);
            }
            writer.Flush();
        }

        //
        //  This function will do a Regex.Replace, but will automatically keep any "match groups" in the 
        //  pattern if no match group variables are specified in the replacement string.  If match group
        //  variables exist in the replacement string, a regular Regex.Replace will be performed.
        //  
        //  Example:    DoRegexReplaceWithAutoSave("ABCDEFGHI", "ABCDEFGHI", "XXX")
        //  Result :    XXX
        //  Reason :    Performs normal Regex replacement since no match groups were specified.
        //
        //  Example:    DoRegexReplaceWithAutoSave("ABCDEFGHI", "(ABC)DEF(GHI)", "XXX")
        //  Result :    ABCXXXGHI
        //  Reason :    Auto-saves ABC and GHI because they are in groups and no specific group match variable was specified.
        //
        //  Example:    DoRegexReplaceWithAutoSave("ABCDEFGHI", "(ABC)DEF(GHI)", "${1}XXX")
        //  Result :    ABCXXX
        //  Reason :    Saves ABC only because a group match variable was specified (${1}) and normal Regex replacement occurs.
        //
        //
        static public String DoRegexReplaceWithAutoSave(String input, String pattern, String replacement, RegexOptions options)
        {
            System.Text.RegularExpressions.Match match = Regex.Match(input, pattern, options);
            bool autoSaveGroups = !(replacement.Contains("$0") || replacement.Contains("${0}"));

            String matchedString = match.Groups[0].Value;

            for (int i = 1; i < match.Groups.Count && autoSaveGroups; i++)
            {
                // If replacement string contains a match group variable (ex: $1 or ${1}), then stop processing and 
                // just perform a normal regular expression match.
                if (replacement.Contains("$" + i) || replacement.Contains("${" + i + "}"))
                {
                    autoSaveGroups = false;
                    break;
                }

                String matchGroup = match.Groups[i].Value;
                // Escape Any Regex Special Characters
                matchGroup = matchGroup.Replace(@"\", @"\\");
                matchGroup = matchGroup.Replace(@"^", @"\^");
                matchGroup = matchGroup.Replace(@"$", @"\$");
                matchGroup = matchGroup.Replace(@".", @"\.");
                matchGroup = matchGroup.Replace(@"|", @"\|");
                matchGroup = matchGroup.Replace(@"?", @"\?");
                matchGroup = matchGroup.Replace(@"*", @"\*");
                matchGroup = matchGroup.Replace(@"+", @"\+");
                matchGroup = matchGroup.Replace(@"(", @"\(");
                matchGroup = matchGroup.Replace(@")", @"\)");
                matchGroup = matchGroup.Replace(@"[", @"\[");
                matchGroup = matchGroup.Replace(@"]", @"\]");

                // Remove the match group value from the string you will eventually match on
                matchedString = Regex.Replace(matchedString, matchGroup + "?", "");
            }

            if (autoSaveGroups)
                return Regex.Replace(input, matchedString, replacement, options);
            else
                return Regex.Replace(input, pattern, replacement, options);

        }

    }
}
