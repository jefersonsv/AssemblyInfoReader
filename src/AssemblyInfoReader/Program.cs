using AssemblyInfoReader;
using CommandLineParser.Arguments;
using CommandLineParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyInfoReader.Demo
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            CommandLineParser.CommandLineParser parser = new CommandLineParser.CommandLineParser();

            ValueArgument<string> assembly = new ValueArgument<string>(
                'a', "assembly", "Specify the assembly file");
            assembly.Optional = false;

            SwitchArgument silent = new SwitchArgument(
                's', "silent", "Enable silent mode", false);

            EnumeratedValueArgument<string> output = new EnumeratedValueArgument<string>(
                'o', "output", new string[] { "none", "screen", "environment-machine", "environment-user", "environment-process" });
            output.DefaultValue = "none";

            ValueArgument<string> name = new ValueArgument<string>(
                'n', "name", "Name of environment to create");

            parser.Arguments.Add(assembly);
            parser.Arguments.Add(silent);
            parser.Arguments.Add(output);
            parser.Arguments.Add(name);

            if (args.Length == 0)
            {
                parser.ShowUsage();
            }
            else
            {
                try
                {
                    parser.ParseCommandLine(args);

                    if (!string.Equals(output.Value, "none", StringComparison.OrdinalIgnoreCase) && !string.Equals(output.Value, "screen", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(name.Value))
                            throw new Exception("The name parameter must be informed");
                    }

                    var version = GetInfo.GetFileVersionFrom(assembly.Value);

                    switch (output.Value.ToLower())
                    {
                        case "screen":
                            Console.Write(version);
                            break;

                        case "environment-machine":
                            EnvironmentHelper.SetEnvironment(name.Value, version, EnvironmentVariableTarget.Machine);
                            break;

                        case "environment-user":
                            EnvironmentHelper.SetEnvironment(name.Value, version, EnvironmentVariableTarget.User);
                            break;

                        case "environment-process":
                            EnvironmentHelper.SetEnvironment(name.Value, version, EnvironmentVariableTarget.Process);
                            break;
                    }

                    if (!silent.Value)
                    {
                        Console.WriteLine($"Assembly File Version {version}");
                    }

                    Environment.Exit(0); // Exit with successful
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Environment.Exit(2); // Exit default error
                }
            }

            Environment.Exit(1); // Exit without execute
        }
    }
}