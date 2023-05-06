using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assembly.Operations;

namespace Assembly
{
    public static class Assembly
    {
        public enum Command
        {
            run,
            quit,
            create,
            help,
            read,
            write,
        }
        public enum OperationType
        {
            none,
            add, // A = Add(B, C) A = B + C
            sub, // A = Subtract(B, C) A = B - C
            mult, // A = Multiply(B, C) = B * C
            div, // A = Divide(B, C) = B / C
            shiftleft,
            shiftright,
            set, // A = LoadImmediate(value) A = value
            inc, // A = Increase(B) A = B + 1
            dec, // A = Decrease(B) A = B - 1
            inv, // A = Invert(A) A = -A
            jump, // Jump(A) Load operation at Index A
            jiz, // JumpIfZero(A, B) Load operation at Index A if B is 0
            save, // Store(A, B) Store Data from Index A in Active Register at Index B of Storage Register
            load, // Load(A, B) Load Data from Index A of Storage Register into Index B of Active Register
            halt, // Ends the program
            stop, // Stop adding new operations
        }

        public static Register activeRegister { get; private set; }
        public static Register storageRegister { get; private set; }
        public static List<Operation> fullProgram { get; private set; }
        public static int ProgramIndex { get; set; }
        private static bool programRunning;

        private const int activeRegisterSize = 8;
        private const int storageRegisterSize = 64;
        private const string fileName = "StorageRegister.csv";

        public static void Main()
        {
            bool running = true;
            activeRegister = new Register(activeRegisterSize);
            storageRegister = new Register(storageRegisterSize);
            fullProgram = new List<Operation>();

            LoadRegister(storageRegister, fileName);

            IntroduceProgram();
            while (running)
            {
                Console.WriteLine();
                ListenForInput(ref running);
            }
            SaveRegister(storageRegister, fileName);
        }

        public static void SaveRegister(Register register, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (bool[] binaryNumber in register.Data)
                {
                    string binaryString = string.Join(",", binaryNumber.Reverse().Select(b => b ? "true" : "false"));
                    writer.WriteLine(binaryString);
                }
            }
        }

        private static void LoadRegister(Register register, string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"No Data at {filePath}: Storage Register Will Begin Empty");
                Console.ResetColor();
                return;
            }

            string[] allLines = File.ReadAllLines(filePath);

            for (int i = 0; i < allLines.Length; i++)
            {
                string[] binaryStrings = allLines[i].Split(',');
                bool[] binaryValues = new bool[Register.BITS];
                for (int j = 0; j < binaryStrings.Length; j++)
                {
                    bool.TryParse(binaryStrings[j], out binaryValues[j]);
                }
                register.Data[i] = binaryValues;
            }
        }

        public static void EndProgram()
        {
            programRunning = false;
        }

        private static void IntroduceProgram()
        {
            Console.WriteLine("Welcome to the assembly system by Lucas Ackman");
            Console.WriteLine("Write whatever program you want.");
            Console.WriteLine($"Type \"{nameof(Command.help)}\" if you need help!");
        }

        private static void ListenForInput(ref bool running)
        {
            string userInput = Console.ReadLine()?.ToLower();

            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("Please Enter Some command");
                return;
            }
            Console.WriteLine();
            // User Entered A Command
            switch (userInput)
            {
                case nameof(Command.run):
                    RunProgram();
                    break;
                case nameof(Command.quit):
                    running = false;
                    break;
                case nameof(Command.create):
                    CreateProgram();
                    break;
                case nameof(Command.help):
                    ShowCommands();
                    break;
                default:
                    UnrecognizedCommand();
                    break;
            }
        }

        private static void RunProgram()
        {
            ProgramIndex = 0;
            programRunning = true;
            while (programRunning)
            {
                var operation = fullProgram[ProgramIndex];
                ProgramIndex++;
                operation.Operate();
            }
            Register.ConvertToInteger(storageRegister.Data[0], out int value);
            Console.WriteLine($"Final Value at {value}");
        }

        private static void CreateProgram()
        {
            ShowOperations();

            fullProgram = new List<Operation>();
            AddOperations();
        }

        private static void ShowOperations()
        {
            Console.WriteLine("The Operation Types Are:");
            Console.WriteLine($"{nameof(OperationType.set)} (Set a value)");
            Console.WriteLine($"{nameof(OperationType.add)} (Add two values)");
            Console.WriteLine($"{nameof(OperationType.sub)} (Subtract two values)");
            Console.WriteLine($"{nameof(OperationType.mult)} (Multiply two values)");
            Console.WriteLine($"{nameof(OperationType.div)} (Divide two values)");
            Console.WriteLine($"{nameof(OperationType.inc)} (Increase a value by 1)");
            Console.WriteLine($"{nameof(OperationType.dec)} (Decrease a value by 1)");
            Console.WriteLine($"{nameof(OperationType.jump)} (Jump to the Operation at Index A)");
            Console.WriteLine($"{nameof(OperationType.jiz)} (Jump to the Operation at Index A if B is 0)");
            Console.WriteLine($"{nameof(OperationType.load)} (Load A from Index B of Storage Register");
            Console.WriteLine($"{nameof(OperationType.save)} (Store A at Index B of Storage Register)");
            Console.WriteLine($"{nameof(OperationType.shiftleft)} (Shift the binary data left across the array)");
            Console.WriteLine($"{nameof(OperationType.shiftright)} (Shift the binary data right across the array)");
            Console.WriteLine($"{nameof(OperationType.halt)} (Ends the Program");
            Console.WriteLine($"{nameof(OperationType.stop)} (Stop adding new operations to the program)");
        }

        private static void AddOperations()
        {
            bool addingOperations = true;
            while (addingOperations)
            {
                AssignOperation(out var operationType);

                switch (operationType)
                {
                    case OperationType.add:
                    {
                        Console.Write("Reading Index A: ");
                        int.TryParse(Console.ReadLine(), out int readingIndexA);
                        if (readingIndexA < 0 || readingIndexA > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        Console.Write("Reading Index B: ");
                        int.TryParse(Console.ReadLine(), out int readingIndexB);
                        if (readingIndexB < 0 || readingIndexB > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        Console.Write("Write to Index: ");
                        int.TryParse(Console.ReadLine(), out int writingIndex);
                        if (writingIndex < 0 || writingIndex > activeRegister.Data.Length - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        fullProgram.Add(new AddOperation(readingIndexA, readingIndexB, writingIndex));
                        break;
                    }
                    case OperationType.sub:
                    {
                        Console.Write("Reading Index A: ");
                        int.TryParse(Console.ReadLine(), out int readingIndexA);
                        if (readingIndexA < 0 || readingIndexA > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        Console.Write("Reading Index B: ");
                        int.TryParse(Console.ReadLine(), out int readingIndexB);
                        if (readingIndexB < 0 || readingIndexB > activeRegister.Data.Length - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        Console.Write("Write to Index: ");
                        int.TryParse(Console.ReadLine(), out int writingIndex);
                        if (writingIndex < 0 || writingIndex > activeRegister.Data.Length - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        fullProgram.Add(new SubtractOperation(readingIndexA, readingIndexB, writingIndex));
                        break;
                    }
                    case OperationType.mult:
                    {
                        Console.Write("Base Index: ");
                        int.TryParse(Console.ReadLine(), out int baseIndex);
                        if (baseIndex < 0 || baseIndex > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        Console.Write("Factor Index: ");
                        int.TryParse(Console.ReadLine(), out int factorIndex);
                        if (factorIndex < 0 || factorIndex > activeRegister.Data.Length - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        Console.Write("Write to Index: ");
                        int.TryParse(Console.ReadLine(), out int writeToIndex);
                        if (writeToIndex < 0 || writeToIndex > activeRegister.Data.Length - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        fullProgram.Add(new MultiplicationOperation(baseIndex, factorIndex, writeToIndex));
                        break;
                    }
                    case OperationType.div:
                    {
                        Console.Write("Numerator Index: ");
                        int.TryParse(Console.ReadLine(), out int numeratorIndex);
                        if (numeratorIndex < 0 || numeratorIndex > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        Console.Write("Denominator Index: ");
                        int.TryParse(Console.ReadLine(), out int denominatorIndex);
                        if (denominatorIndex < 0 || denominatorIndex > activeRegister.Data.Length - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        Console.Write("Write to Index: ");
                        int.TryParse(Console.ReadLine(), out int writeToIndex);
                        if (writeToIndex < 0 || writeToIndex > activeRegister.Data.Length - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        fullProgram.Add(new DivisionOperation(numeratorIndex, denominatorIndex, writeToIndex));
                        break;
                    }
                    case OperationType.inc:
                    {
                        Console.Write("Increase Index: ");
                        int.TryParse(Console.ReadLine(), out int increasedIndex);
                        if (increasedIndex < 0 || increasedIndex > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        fullProgram.Add(new IncreaseOperation(increasedIndex));
                        break;
                    }
                    case OperationType.dec:
                    {
                        Console.Write("Decrease Index: ");
                        int.TryParse(Console.ReadLine(), out int decreasedIndex);
                        if (decreasedIndex < 0 || decreasedIndex > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        fullProgram.Add(new DecreaseOperation(decreasedIndex));
                        break;
                    }
                    case OperationType.inv:
                    {
                        Console.Write("Invert Index: ");
                        int.TryParse(Console.ReadLine(), out int invertedIndex);
                        if (invertedIndex < 0 || invertedIndex > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        fullProgram.Add(new InvertOperation(invertedIndex));
                        break;
                    }
                    case OperationType.save:
                    {
                        Console.Write("Save From Index: ");
                        int.TryParse(Console.ReadLine(), out int saveFromIndex);
                        if (saveFromIndex < 0 || saveFromIndex > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        Console.Write("Save To Index: ");
                        int.TryParse(Console.ReadLine(), out int saveToIndex);
                        if (saveToIndex < 0 || saveToIndex > storageRegisterSize - 1)
                        {
                            InputOutOfRange(0, storageRegisterSize - 1);
                            break;
                        }
                        fullProgram.Add(new SaveOperation(saveFromIndex, saveToIndex));
                        break;
                    }
                    case OperationType.load:
                    {
                        Console.Write("Load From Index: ");
                        int.TryParse(Console.ReadLine(), out int loadFromIndex);
                        if (loadFromIndex < 0 || loadFromIndex > storageRegisterSize - 1)
                        {
                            InputOutOfRange(0, storageRegisterSize - 1);
                            break;
                        }
                        Console.Write("Load To Index: ");
                        int.TryParse(Console.ReadLine(), out int loadToIndex);
                        if (loadToIndex < 0 || loadToIndex > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        fullProgram.Add(new LoadOperation(loadFromIndex, loadToIndex));
                        break;
                    }
                    case OperationType.set:
                    {
                        Console.Write("Value: ");
                        int.TryParse(Console.ReadLine(), out int value);
                        if (value < Register.minValue || value > Register.maxValue)
                        {
                            InputOutOfRange(Register.minValue, Register.maxValue);
                            break;
                        }
                        Console.Write("Set To Index: ");
                        int.TryParse(Console.ReadLine(), out int setToIndex);
                        if (setToIndex < 0 || setToIndex > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        fullProgram.Add(new SetOperation(value, setToIndex));
                        break;
                    }
                    case OperationType.jiz:
                    {
                        Console.Write("Jump to Index: ");
                        int.TryParse(Console.ReadLine(), out int jumpToIndex);
                        if (jumpToIndex < 0 || jumpToIndex > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        Console.Write("Check Value at Index: ");
                        int.TryParse(Console.ReadLine(), out int zeroCheckIndex);
                        if (zeroCheckIndex < 0 || zeroCheckIndex > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        fullProgram.Add(new JumpIfZeroOperation(jumpToIndex, zeroCheckIndex));
                        break;
                }
                    case OperationType.jump:
                    {
                        Console.Write("Jump to Index: ");
                        int.TryParse(Console.ReadLine(), out int jumpToIndex);
                        if (jumpToIndex < 0 || jumpToIndex > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        fullProgram.Add(new JumpOperation(jumpToIndex));
                        break;
                    }
                    case OperationType.halt:
                    {
                        fullProgram.Add(new HaltOperation());
                        break;
                    }
                    case OperationType.stop:
                    {
                        addingOperations = false;
                        break;
                    }
                    case OperationType.none:
                    {
                        break;
                    }
                    case OperationType.shiftleft:
                    {
                        Console.Write("Shift Left at index: ");
                        int.TryParse(Console.ReadLine(), out int shiftIndex);
                        if (shiftIndex < 0 || shiftIndex > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        fullProgram.Add(new ShiftOperation(shiftIndex, shiftLeft: true));
                        break;
                    }
                    case OperationType.shiftright:
                    {
                        Console.Write("Shift Right at index: ");
                        int.TryParse(Console.ReadLine(), out int shiftIndex);
                        if (shiftIndex < 0 || shiftIndex > activeRegisterSize - 1)
                        {
                            InputOutOfRange(0, activeRegisterSize - 1);
                            break;
                        }
                        fullProgram.Add(new ShiftOperation(shiftIndex, shiftRight: true));
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(operationType),
                            operationType,
                            $"{nameof(operationType)} must be between {OperationType.none} and {OperationType.stop}"
                        );
                }
            }
        }

        private static void InputOutOfRange(int minValue, int maxValue)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Value inputted must be between {minValue} and {maxValue}");
            Console.WriteLine("Construction of Operation has been Cancelled, Please Try Again.");
            Console.ResetColor();
        }

        public static void AssignOperation(out OperationType operationType)
        {
            Console.WriteLine();
            Console.WriteLine("Enter The Operation Being Added:");
            string userInput = Console.ReadLine()?.ToLower();

            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("ERROR: Missing Operation Input");
            }

            operationType = OperationType.none;

            switch (userInput)
            {
                case nameof(OperationType.add):
                    operationType = OperationType.add;
                    break;
                case nameof(OperationType.sub):
                    operationType = OperationType.sub;
                    break;
                case nameof(OperationType.mult):
                    operationType = OperationType.mult;
                    break;
                case nameof(OperationType.div):
                    operationType = OperationType.div;
                    break;
                case nameof(OperationType.set):
                    operationType = OperationType.set;
                    break;
                case nameof(OperationType.inc):
                    operationType = OperationType.inc;
                    break;
                case nameof(OperationType.dec):
                    operationType = OperationType.dec;
                    break;
                case nameof(OperationType.inv):
                    operationType = OperationType.inv;
                    break;
                case nameof(OperationType.jump):
                    operationType = OperationType.jump;
                    break;
                case nameof(OperationType.jiz):
                    operationType = OperationType.jiz;
                    break;
                case nameof(OperationType.save):
                    operationType = OperationType.save;
                    break;
                case nameof(OperationType.load):
                    operationType = OperationType.load;
                    break;
                case nameof(OperationType.halt):
                    operationType = OperationType.halt;
                    break;
                case nameof(OperationType.stop):
                    operationType = OperationType.stop;
                    break;
                case nameof(OperationType.shiftleft):
                    operationType = OperationType.shiftleft;
                    break;
                case nameof(OperationType.shiftright):
                    operationType = OperationType.shiftright;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nOperation Not Found, Please Try Again\n");
                    Console.ResetColor();
                    ShowOperations();
                    break;
            }
        }

        private static void ShowCommands()
        {
            Console.WriteLine("The Available Commands Are:");
            Console.WriteLine($"{nameof(Command.create)} (Create a new Program)");
            Console.WriteLine($"{nameof(Command.run)} (Run the Current Program)");
            Console.WriteLine($"{nameof(Command.quit)} (Quit the System)");
            Console.WriteLine($"{nameof(Command.read)} (Read a program from a saved file)");
            Console.WriteLine($"{nameof(Command.write)} (Write the current program to a file)");
        }

        private static void UnrecognizedCommand()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Command Not Recognized\n");
            Console.ResetColor();
            ShowCommands();
        }
    }
}
