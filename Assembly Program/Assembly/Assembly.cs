using System;
using System.Collections.Generic;
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
        }
        public enum OperationType
        {
            none,
            add, // A = Add(B, C) A = B + C
            sub, // A = Subtract(B, C) A = B - C
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

        public static Register activeRegister;
        public static Register storageRegister;
        public static List<Operation> fullProgram;
        public static int programIndex;
        public static bool programRunning;

        public const int activeRegisterSize = 8;
        public const int storageRegisterSize = 64;

        public static void Main()
        {
            bool running = true;
            activeRegister = new Register(activeRegisterSize);
            storageRegister = new Register(storageRegisterSize);
            fullProgram = new List<Operation>();

            IntroduceProgram();
            while (running)
            {
                Console.WriteLine();
                ListenForInput(ref running);
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
            programIndex = 0;
            programRunning = true;
            while (programRunning)
            {
                var operation = fullProgram[programIndex];
                programIndex++;
                operation.Operate();
            }
            Register.ConvertToInteger(storageRegister.Data[0], out int value);
            Console.WriteLine($"Final Value at {value}");
        }

        private static void CreateProgram()
        {
            Console.WriteLine("The Operation Types Are:");
            Console.WriteLine($"{nameof(OperationType.set)} (Set a value)");
            Console.WriteLine($"{nameof(OperationType.add)} (Add two values)");
            Console.WriteLine($"{nameof(OperationType.sub)} (Subtract two values)");
            Console.WriteLine($"{nameof(OperationType.inc)} (Increase a value by 1)");
            Console.WriteLine($"{nameof(OperationType.dec)} (Decrease a value by 1)");
            Console.WriteLine($"{nameof(OperationType.jump)} (Jump to the Operation at Index A)");
            Console.WriteLine($"{nameof(OperationType.jiz)} (Jump to the Operation at Index A if B is 0)");
            Console.WriteLine($"{nameof(OperationType.load)} (Load A from Index B of Storage Register");
            Console.WriteLine($"{nameof(OperationType.save)} (Store A at Index B of Storage Register)");
            Console.WriteLine($"{nameof(OperationType.halt)} (Ends the Program");
            Console.WriteLine($"{nameof(OperationType.stop)} (Stop adding new operations to the program)");

            fullProgram = new List<Operation>();
            AddOperations();
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
                        Console.Write("Reading Index B: ");
                        int.TryParse(Console.ReadLine(), out int readingIndexB);
                        Console.Write("Writing Index: ");
                        int.TryParse(Console.ReadLine(), out int writingIndex);
                        fullProgram.Add(new AddOperation(readingIndexA, readingIndexB, writingIndex));
                        break;
                    }
                    case OperationType.sub:
                    {
                        Console.Write("Reading Index A: ");
                        int.TryParse(Console.ReadLine(), out int readingIndexA);
                        Console.Write("Reading Index B: ");
                        int.TryParse(Console.ReadLine(), out int readingIndexB);
                        Console.Write("Writing Index: ");
                        int.TryParse(Console.ReadLine(), out int writingIndex);
                        fullProgram.Add(new SubtractOperation(readingIndexA, readingIndexB, writingIndex));
                        break;
                    }
                    case OperationType.inc:
                    {
                        Console.Write("Increase Index: ");
                        int.TryParse(Console.ReadLine(), out int increasedIndex);
                        fullProgram.Add(new IncreaseOperation(increasedIndex));
                        break;
                    }
                    case OperationType.dec:
                    {
                        Console.Write("Decrease Index: ");
                        int.TryParse(Console.ReadLine(), out int decreasedIndex);
                        fullProgram.Add(new DecreaseOperation(decreasedIndex));
                        break;
                    }
                    case OperationType.inv:
                    {
                        Console.Write("Invert Index: ");
                        int.TryParse(Console.ReadLine(), out int invertedIndex);
                        fullProgram.Add(new InvertOperation(invertedIndex));
                        break;
                    }
                    case OperationType.save:
                    {
                        Console.Write("Save From Index: ");
                        int.TryParse(Console.ReadLine(), out int saveFromIndex);
                        Console.Write("Save To Index: ");
                        int.TryParse(Console.ReadLine(), out int saveToIndex);
                        fullProgram.Add(new SaveOperation(saveFromIndex, saveToIndex));
                        break;
                    }
                    case OperationType.load:
                    {
                        Console.Write("Load From Index: ");
                        int.TryParse(Console.ReadLine(), out int loadFromIndex);
                        Console.Write("Load To Index: ");
                        int.TryParse(Console.ReadLine(), out int loadToIndex);
                        fullProgram.Add(new LoadOperation(loadFromIndex, loadToIndex));
                        break;
                    }
                    case OperationType.set:
                    {
                        Console.Write("Value: ");
                        int.TryParse(Console.ReadLine(), out int value);
                        Console.Write("Set To Index: ");
                        int.TryParse(Console.ReadLine(), out int setToIndex);
                        fullProgram.Add(new SetOperation(value, setToIndex));
                        break;
                    }
                    case OperationType.jiz:
                    {
                        Console.Write("Jump to Index: ");
                        int.TryParse(Console.ReadLine(), out int jumpToIndex);
                        Console.Write("Storage Index: ");
                        int.TryParse(Console.ReadLine(), out int zeroCheckIndex);
                        fullProgram.Add(new JumpIfZeroOperation(jumpToIndex, zeroCheckIndex));
                        break;
                    }
                    case OperationType.jump:
                    {
                        Console.Write("Jump to Index: ");
                        int.TryParse(Console.ReadLine(), out int jumpToIndex);
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
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(operationType),
                            operationType,
                            $"{nameof(operationType)} must be between {OperationType.none} and {OperationType.stop}"
                        );
                }
            }
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
                default:
                    Console.WriteLine("Operation Not Found");
                    break;
            }
        }

        private static void ShowCommands()
        {
            Console.WriteLine("The Available Commands Are:");
            Console.WriteLine($"{nameof(Command.create)} (Create a new Program)");
            Console.WriteLine($"{nameof(Command.run)} (Run the Current Program)");
            Console.WriteLine($"{nameof(Command.quit)} (Quit the System)");
        }

        private static void UnrecognizedCommand()
        {
            Console.WriteLine("Command Not Recognized");
            ShowCommands();
        }
    }
}
