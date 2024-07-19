namespace Number_System_Base_Conversion
{
    internal class Program
    {
        static void Main()
        {
            const string initialBaseMsg = "Enter the initial base to convert from. The base must be an integer from 2 to 36:";
            const string finalBaseMsg = "\nEnter the final base to convert to. The base must be an integer from 2 to 36:";
            const string numberMsg = "\nEnter the number you wish to convert:";
            char[] digitsLibrary = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
                'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            bool quit = false;

            while (!quit)
            {
                int initialBase = ValidateBase(initialBaseMsg);

                int finalBase = ValidateBase(finalBaseMsg);

                ConvertNumber(initialBase, finalBase, numberMsg);

                quit = QuitOrNot();
                Console.Write("\n");
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

            void ConvertNumber(int initialBase, int finalBase, string message)
            {
                string input = "";
                bool numberIsNegative = false;
                double value = 0;

                while (true)
                {
                    Console.WriteLine(message);
                    input = Console.ReadLine().ToUpper();

                    if (ValidateThenCompute(initialBase, input, out value, out numberIsNegative))
                    {
                        break;
                    }

                    else
                    {
                        Console.WriteLine("\nError: invalid value.\n");
                    }
                }

                string output = ComputeThenConvert(finalBase, value);

                if (input.Contains(","))
                {
                    output = output.Replace(".", ",");
                }

                output = (numberIsNegative ? "-" + output : output);

                Console.WriteLine($"\nHere is the converted number:\n{output}");
            }

            bool ValidateThenCompute(int initialBase, string input, out double value, out bool numberIsNegative)
            {
                numberIsNegative = false;
                value = 0;
                int positionDotOrComma = -1;

                if (input == "")
                {
                    return false;
                }

                if (input.StartsWith('-'))
                {
                    input = input.Substring(1);
                    numberIsNegative = true;
                }
                
                else if (input.StartsWith("+"))
                {
                    input = input.Substring(1);
                }

                if (!(input.Contains(",") && input.Contains(".")) && (input.Contains(",") || input.Contains(".")))
                {
                    char c = (input.Contains(",") ? ',' : '.');

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

                else if (input.Contains(",") && input.Contains("."))
                {
                    return false;
                }

                int exponent = (positionDotOrComma == -1 ? 0 : positionDotOrComma + 1 - input.Length);

                for (int position = input.Length - 1; position >= 0; position--)
                {
                    if (position == positionDotOrComma) { continue; }

                    char c = input[position];
                    int tempValue = -1;

                    for (int counter = 0; counter < initialBase; counter++)
                    {
                        if (digitsLibrary[counter] == c)
                        {
                            tempValue = counter;

                            break;
                        }

                        else if (counter == initialBase - 1)
                        {
                            return false;
                        }
                    }

                    value += tempValue * Math.Pow(initialBase, exponent);
                    exponent++;

                }

                return true;
            }

            string ComputeThenConvert(int finalBase, double value)
            {
                string output = "";
                double integerPart = Math.Truncate(value);
                double fractionPart = value - integerPart;

                if (fractionPart != 0)
                {
                    int precisionLimit = 0;

                    while (fractionPart > 0 && precisionLimit < 15)
                    {
                        int integerValue = (int) Math.Floor(fractionPart * finalBase);
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
                    int residue = (int) integerPart % finalBase;
                    output = digitsLibrary[residue] + output;
                    integerPart = Math.Floor(integerPart / finalBase);
                }

                return output;
            }

            bool QuitOrNot()
            {
                Console.WriteLine("\nDo you want to continue? Hit Enter to continue, or write \"q\" and then press Enter to quit:");

                return ((Console.ReadLine() == "q") ? true : false);
            }
        }
    }
}