using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjektWektor3D
{
    public class Commands
    {
        private static readonly Regex OperationsRegex = new Regex(@"[+\-*\.^|\/]", RegexOptions.Compiled);

        private static readonly Regex SpecialOperationsRegex = new Regex(@"(\w+)\(([^)]*)\)", RegexOptions.Compiled);

        public static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }

        public static bool ValidateName(string name)
        {
            foreach (var c in name)
                if (!char.IsLetterOrDigit(c) && c != ',')
                    return false;

            return true;
        }

        public string ParseCommand(string command, Dictionary<string, Vector3D> vectors)
        {
            var trimmedCommand = RemoveWhitespace(command);
            if (!trimmedCommand.Contains('='))
                throw new Exception("Invalid command!");


            var splittedCommand = trimmedCommand.Split('=');
            if (splittedCommand.Length != 2)
                throw new Exception("Invalid command!");

            var resultName = splittedCommand[0];

            var restOfCommand = splittedCommand[1];

            var matches = OperationsRegex.Matches(restOfCommand);
            if (matches.Count == 1)
            {
                var operation = restOfCommand[matches[0].Index];
                var firstVariableName = restOfCommand.Substring(0, matches[0].Index);
                var secondVariableName = restOfCommand.Substring(matches[0].Index + 1);

                if (!ValidateName(firstVariableName))
                    throw new Exception("Invalid command name!");

                if (!ValidateName(secondVariableName))
                    throw new Exception("Invalid command name!");

                switch (operation)
                {
                    case '+':
                    {
                        if (!vectors.ContainsKey(firstVariableName))
                            throw new Exception("Invalid command!");

                        if (!vectors.ContainsKey(secondVariableName))
                            throw new Exception("Invalid command!");

                        vectors[resultName] = vectors[firstVariableName] + vectors[secondVariableName];
                        return $@"{resultName} = {vectors[resultName]}";
                    }

                    case '-':
                    {
                        if (!vectors.ContainsKey(firstVariableName))
                            throw new Exception("Invalid command!");

                        if (!vectors.ContainsKey(secondVariableName))
                            throw new Exception("Invalid command!");

                        vectors[resultName] = vectors[firstVariableName] - vectors[secondVariableName];
                        return $@"{resultName} = {vectors[resultName]}";
                    }

                    case '*':
                    {
                        if (!vectors.ContainsKey(firstVariableName))
                            throw new Exception("Invalid command!");

                        if (!double.TryParse(secondVariableName, NumberStyles.Any, CultureInfo.InvariantCulture,
                                out var scalar))
                            throw new Exception("Invalid command!");

                        vectors[resultName] = vectors[firstVariableName] * scalar;
                        return $@"{resultName} = {vectors[resultName]}";
                    }

                    case '/':
                    {
                        if (!vectors.ContainsKey(firstVariableName))
                            throw new Exception("Invalid command!");

                        if (!double.TryParse(secondVariableName, NumberStyles.Any, CultureInfo.InvariantCulture,
                                out var scalar))
                            throw new Exception("Invalid command!");

                        vectors[resultName] = vectors[firstVariableName] / scalar;
                        return $@"{resultName} = {vectors[resultName]}";
                    }

                    case '.':
                    {
                        if (!vectors.ContainsKey(firstVariableName))
                            throw new Exception("Invalid command!");

                        if (!vectors.ContainsKey(secondVariableName))
                            throw new Exception("Invalid command!");

                        var result = vectors[firstVariableName].DotProduct(vectors[secondVariableName]);
                        return $@"{resultName} = {result}";
                    }

                    case '^':
                    {
                        if (!vectors.ContainsKey(firstVariableName))
                            throw new Exception("Invalid command!");

                        if (!vectors.ContainsKey(secondVariableName))
                            throw new Exception("Invalid command!");

                        vectors[resultName] = vectors[firstVariableName].CrossProduct(vectors[secondVariableName]);
                        return $@"{resultName} = {vectors[resultName]}";
                    }

                    default:
                        throw new Exception("Invalid command!");
                }
            }

            if (matches.Count == 2)
            {
                var operation = restOfCommand[matches[0].Index];
                if (operation != '|')
                    throw new Exception("Invalid command!");

                var firstVariableName =
                    restOfCommand.Substring(matches[0].Index + 1, restOfCommand.Length - matches[1].Index + 1);

                if (!ValidateName(firstVariableName))
                    throw new Exception("Invalid command name!");

                if (!vectors.ContainsKey(firstVariableName))
                    throw new Exception("Invalid command!");

                var result = vectors[firstVariableName].Magnitude();
                return $@"{resultName} = {result}";
            }

            var specialMatches = SpecialOperationsRegex.Matches(restOfCommand);
            if (specialMatches.Count != 1)
                throw new Exception("Invalid command!");

            if (specialMatches[0].Groups.Count != 3)
                throw new Exception("Invalid command!");


            var operationName = specialMatches[0].Groups[1].Value;
            if (!ValidateName(operationName))
                throw new Exception("Invalid command name!");

            var operationArguments = specialMatches[0].Groups[2].Value;

            string[] splittedArguments;
            if (operationArguments.Contains(','))
            {
                splittedArguments = operationArguments.Split(',');
                foreach (var argument in splittedArguments)
                    if (!ValidateName(argument))
                        throw new Exception("Invalid command name!");
            }
            else
            {
                splittedArguments = new string[1];
                splittedArguments[0] = operationArguments;

                if (!ValidateName(operationArguments))
                    throw new Exception("Invalid command!");
            }

            switch (operationName)
            {
                case "Normalize":
                {
                    if (splittedArguments.Length != 1)
                        throw new Exception("Invalid command!");

                    if (!vectors.ContainsKey(splittedArguments[0]))
                        throw new Exception("Invalid command!");

                    vectors[resultName] = vectors[splittedArguments[0]].Normalize();
                    return $@"{resultName} = {vectors[resultName]}";
                }

                case "Negation":
                {
                    if (splittedArguments.Length != 1)
                        throw new Exception("Invalid command!");

                    if (!vectors.ContainsKey(splittedArguments[0]))
                        throw new Exception("Invalid command!");

                    vectors[resultName] = -vectors[splittedArguments[0]];
                    return $@"{resultName} = {vectors[resultName]}";
                }

                case "Distance":
                {
                    if (splittedArguments.Length != 2)
                        throw new Exception("Invalid command!");

                    if (!vectors.ContainsKey(splittedArguments[0]))
                        throw new Exception("Invalid command!");

                    if (!vectors.ContainsKey(splittedArguments[1]))
                        throw new Exception("Invalid command!");

                    var result = vectors[splittedArguments[0]].Distance(vectors[splittedArguments[1]]);
                    return $@"{resultName} = {result}";
                }

                case "Angle":
                {
                    if (splittedArguments.Length != 2)
                        throw new Exception("Invalid command!");

                    if (!vectors.ContainsKey(splittedArguments[0]))
                        throw new Exception("Invalid command!");

                    if (!vectors.ContainsKey(splittedArguments[1]))
                        throw new Exception("Invalid command!");

                    var result = vectors[splittedArguments[0]].Angle(vectors[splittedArguments[1]]);
                    return $@"{resultName} = {result}";
                }

                default:
                    throw new Exception("Invalid command!");
            }
        }
    }
}