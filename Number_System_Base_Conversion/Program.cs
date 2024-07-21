using System;
using System.Diagnostics.Metrics;

namespace Number_System_Base_Conversion
{
    internal class Program
    {
        static void Main()
        {
            const string initialMsg = "***** Number System Base Converter, for any integer base from to 2 to 36 *****\n\n" +
                    "IMPORTANT NOTE: due to \"Machine Epsilon\" related issues, I have limited the precision of my algorithm, so " +
                    "that the expansion of a floating-point number is truncated after 15 rounds. So the computed values are merely approximative.";
            const string finalMsg = "\nDo you want to continue? Hit Enter to continue, or write \"q\" and then press Enter to quit:";
            const string initialBaseMsg = "\nEnter the initial base to convert from. The base must be an integer from 2 to 36:";
            const string finalBaseMsg = "\nEnter the final base to convert to. The base must be an integer from 2 to 36:";
            const string numberMsg = "\nEnter the number you wish to convert:";

            char[] digitsLibrary = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
                'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            bool quit = false;



            Console.WriteLine(initialMsg);

            while (!quit)
            {
                ConvertNumber();

                Console.WriteLine(finalMsg);

                quit = ((Console.ReadLine() == "q") ? true : false);
                Console.Write("\n");
            }





            void ConvertNumber()
            {
                int initialBase = ValidateBase(initialBaseMsg);
                int finalBase = ValidateBase(finalBaseMsg);
                string input = "";
                bool numberIsNegative = false;
                int positionDotOrComma = -1;
                double value = 0;

                while (true)
                {
                    Console.WriteLine(numberMsg);
                    input = Console.ReadLine().ToUpper();

                    if (input.StartsWith('-'))
                    {
                        input = input.Substring(1);
                        numberIsNegative = true;
                    }

                    else if (input.StartsWith("+"))
                    {
                        input = input.Substring(1);
                    }

                    if (ValidateInput(input, out positionDotOrComma)
                        && ConvertStringToValue(initialBase, input, positionDotOrComma, out value))
                    {
                        break;
                    }

                    else
                    {
                        Console.WriteLine("\nError: invalid value.\n");
                    }
                }

                string output = ConvertValueToString(finalBase, value);

                if (input.Contains(","))
                {
                    output = output.Replace(".", ",");
                }

                output = (numberIsNegative ? "-" + output : output);

                Console.WriteLine($"\nHere is the converted number:\n{output}");
            }

            int ValidateBase(string message)
            {
                int number;

                while (true)
                {
                    Console.WriteLine(message);

                    if (int.TryParse(Console.ReadLine(), out number) && number >= 2 && number <= 36)
                    {
                        break;
                    }

                    else
                    {
                        Console.WriteLine("\nError: invalid value.\n");
                    }
                }

                return number;
            }

            bool ValidateInput(string input, out int positionDotOrComma)
            {
                positionDotOrComma = -1;

                if (input == "")
                {
                    return false;
                }

                if (!(input.Contains(".") && input.Contains(",")) && (input.Contains(".") || input.Contains(",")))
                {
                    char c = (input.Contains(".") ? '.' : ',');

                    int index = input.IndexOf(c);

                    if (index == input.LastIndexOf(c))
                    {
                        positionDotOrComma = index;
                    }

                    else
                    {
                        return false;
                    }
                }

                else if (input.Contains(".") && input.Contains(","))
                {
                    return false;
                }

                return true;
            }

            bool ConvertStringToValue(int initialBase, string input, int positionDotOrComma, out double value)
            {
                value = 0;
                int exponent = (positionDotOrComma == -1 ? 0 : positionDotOrComma + 1 - input.Length);

                for (int position = input.Length - 1; position >= 0; position--)
                {
                    if (position == positionDotOrComma) { continue; }

                    char c = input[position];
                    int tempValue = -1;

                    int index = Array.IndexOf(digitsLibrary, c, 0, initialBase);

                    if (index != -1)
                    {
                        tempValue = index;
                    }

                    else
                    {
                        return false;
                    }

                    value += tempValue * Math.Pow(initialBase, exponent);
                    exponent++;
                }

                return true;
            }

            string ConvertValueToString(int finalBase, double value)
            {
                string output = "";
                double integerPart = Math.Truncate(value);
                double fractionPart = value - integerPart;

                if (fractionPart != 0)
                {
                    int precisionLimit = 0;

                    while (fractionPart > 0 && precisionLimit < 15)
                    {
                        int integerValue = (int) Math.Truncate(fractionPart * finalBase);
                        output += digitsLibrary[integerValue];
                        fractionPart = fractionPart * finalBase - integerValue;

                        precisionLimit++;
                    }

                    output = (output != "" ? "." + output : ".0");
                }

                if (integerPart == 0)
                {
                    output = "0" + output;
                }

                while (integerPart > 0)
                {
                    int residue = (int) (integerPart % finalBase);
                    output = digitsLibrary[residue] + output;
                    integerPart = Math.Floor(integerPart / finalBase);
                }

                return output;
            }
        }
    }
}