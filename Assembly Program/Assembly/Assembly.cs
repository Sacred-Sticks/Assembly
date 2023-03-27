using System;
using System.Collections.Generic;

namespace Assembly
{
    public static class Assembly
    {
        public enum Commands
        {
            run,
            quit,
            create,
            help,
        }

        public static Register activeRegister;
        public static Register storageRegister;
        public static List<ValueSet> activeProgram;
        public static int operationIndex;

        public const int activeRegisterSize = 8;
        public const int storageRegisterSize = 64;
        
        public static void Main()
        {
            bool running = true;
            activeRegister = new Register(activeRegisterSize);
            storageRegister = new Register(storageRegisterSize);
            activeProgram = new List<ValueSet>();
            
            IntroduceProgram();
            while (running)
            {
                Console.WriteLine();
                ListenForInput(ref running);
            }
        }

        private static void IntroduceProgram()
        {
            Console.WriteLine("Welcome to the assembly system by Lucas Ackman");
            Console.WriteLine("Write whatever program you want.");
            Console.WriteLine($"Type \"{nameof(Commands.help)}\" if you need help!");
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
                case nameof(Commands.run):
                    RunProgram();
                    break;
                case nameof(Commands.quit):
                    running = false;
                    break;
                case nameof(Commands.create):
                    CreateProgram();
                    break;
                case nameof(Commands.help):
                    ShowCommands();
                    break;
                default:
                    UnrecognizedCommand();
                    break;
            }
        }

        private static void RunProgram()
        {
            operationIndex = 0;
            while (true)
            {
                var valueSet = activeProgram[operationIndex];
                operationIndex++;
                valueSet.Operation.TriggerOperation(valueSet.StorageIndex, valueSet.ActiveIndices, valueSet.SetValue, out bool continueRunning);
                if (continueRunning)
                    continue;
                Register.ConvertToInteger(storageRegister.Data[0], out int value);
                Console.WriteLine($"Final Value at {value}");
                return;
            }
        }

        private static void CreateProgram()
        {
            Console.WriteLine("The Operation Types Are:");
            Console.WriteLine($"{nameof(Operation.OperationType.set)} (Set a value)");
            Console.WriteLine($"{nameof(Operation.OperationType.add)} (Add two values)");
            Console.WriteLine($"{nameof(Operation.OperationType.sub)} (Subtract two values)");
            Console.WriteLine($"{nameof(Operation.OperationType.inc)} (Increase a value by 1)");
            Console.WriteLine($"{nameof(Operation.OperationType.dec)} (Decrease a value by 1)");
            Console.WriteLine($"{nameof(Operation.OperationType.jump)} (Jump to the Operation at Index A)");
            Console.WriteLine($"{nameof(Operation.OperationType.jiz)} (Jump to the Operation at Index A if B is 0)");
            Console.WriteLine($"{nameof(Operation.OperationType.load)} (Load A from Index B of Storage Register");
            Console.WriteLine($"{nameof(Operation.OperationType.save)} (Store A at Index B of Storage Register)");
            Console.WriteLine($"{nameof(Operation.OperationType.halt)} (Ends the Program");
            Console.WriteLine($"{nameof(Operation.OperationType.stop)} (Stop adding new operations to the program)");

            activeProgram = new List<ValueSet>();
            while (true)
            {

                var operation = new Operation();
                operation.AssignFunction(out bool newOperation);
                int storageIndex = 0;
                int[] activeIndices = new int[3];
                int setValue = 0;

                switch (operation.Function)
                {
                    case Operation.OperationType.add:
                    case Operation.OperationType.sub:
                        Console.Write("Source Index A: ");
                        int.TryParse(Console.ReadLine(), out activeIndices[1]);
                        Console.Write("Source Index B: ");
                        int.TryParse(Console.ReadLine(), out activeIndices[2]);
                        Console.Write("Storage Index: ");
                        int.TryParse(Console.ReadLine(), out activeIndices[0]);
                        break;
                    case Operation.OperationType.inc:
                    case Operation.OperationType.dec:
                    case Operation.OperationType.inv:
                        Console.Write("Source Index: ");
                        int.TryParse(Console.ReadLine(), out activeIndices[0]);
                        break;
                    case Operation.OperationType.save:
                    case Operation.OperationType.load:
                        Console.Write("Source Index: ");
                        int.TryParse(Console.ReadLine(), out activeIndices[0]);
                        Console.Write("Storage Index: ");
                        int.TryParse(Console.ReadLine(), out storageIndex);
                        break;
                    case Operation.OperationType.set:
                    case Operation.OperationType.jiz:
                        Console.Write("Key Value: ");
                        int.TryParse(Console.ReadLine(), out setValue);
                        Console.Write("Storage Index: ");
                        int.TryParse(Console.ReadLine(), out activeIndices[0]);
                        break;
                    case Operation.OperationType.jump:
                        Console.Write("Key Value: ");
                        int.TryParse(Console.ReadLine(), out setValue);
                        break;
                    case Operation.OperationType.none:
                    case Operation.OperationType.halt:
                    case Operation.OperationType.stop:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                var values = new ValueSet(operation, storageIndex, activeIndices, setValue);
                activeProgram.Add(values);
                if (!newOperation)
                    break;
            }
        }

        private static void ShowCommands()
        {
            Console.WriteLine("The Available Commands Are:");
            Console.WriteLine($"{nameof(Commands.create)} (Create a new Program)");
            Console.WriteLine($"{nameof(Commands.run)} (Run the Current Program)");
            Console.WriteLine($"{nameof(Commands.quit)} (Quit the System)");
        }

        private static void UnrecognizedCommand()
        {
            Console.WriteLine("Command Not Recognized");
            ShowCommands();
        }
    }
}
