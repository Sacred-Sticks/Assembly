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
            add,
            remove,
            program,
            assistance,
        }

        public static Register activeRegister;
        public static Register storageRegister;
        public static List<ValueSet> activeProgram;
        public static int operationIndex;
        
        public static void Main()
        {
            bool running = true;
            activeRegister = new Register(8);
            storageRegister = new Register(64);
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
            Console.WriteLine($"Type \"{nameof(Commands.assistance)}\" if you need help!");
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
                case nameof(Commands.add):
                    AddOperation();
                    break;
                case nameof(Commands.remove):
                    RemoveOperation();
                    break;
                case nameof(Commands.program):
                    ShowOperations();
                    break;
                case nameof(Commands.assistance):
                    ShowAssistance();
                    break;
                default:
                    UnrecognizedCommand();
                    break;
            }
        }

        private static void RunProgram()
        {
            bool continueRunning = true;
            operationIndex = 0;
            while (continueRunning)
            {
                var values = activeProgram[operationIndex];
                operationIndex++;
                values.Operation.TriggerOperation(values.StorageIndex, values.ActiveIndices, values.SetValue, out continueRunning);
                if (continueRunning)
                    continue;
                Register.ConvertToInteger(storageRegister.Data[0], out int value);
                Console.WriteLine($"Final Value at {value}");
                return;
            }
        }

        private static void AddOperation()
        {
            var operation = new Operation();
            operation.SetUpOperation();
            int storageIndex = 0;
            int[] activeIndices = new int[2];
            int setValue = 0;
            switch (operation.Function)
            {
                case Operation.OperationType.add:
                case Operation.OperationType.sub:
                    Console.Write("Input Storage Index: ");
                    int.TryParse(Console.ReadLine(), out storageIndex);
                    Console.Write("Input Source Index A: ");
                    int.TryParse(Console.ReadLine(), out activeIndices[0]);
                    Console.Write("Input Source Index B: ");
                    int.TryParse(Console.ReadLine(), out activeIndices[1]);
                    break;
                case Operation.OperationType.inc:
                case Operation.OperationType.dec:
                case Operation.OperationType.str:
                case Operation.OperationType.load:
                    Console.Write("Input Storage Index: ");
                    int.TryParse(Console.ReadLine(), out storageIndex);
                    Console.Write("Input Source Index: ");
                    int.TryParse(Console.ReadLine(), out activeIndices[0]);
                    break;
                case Operation.OperationType.set:
                case Operation.OperationType.jiz:
                    Console.Write("Input Storage Index: ");
                    int.TryParse(Console.ReadLine(), out storageIndex);
                    Console.Write("Input Key Value: ");
                    int.TryParse(Console.ReadLine(), out setValue);
                    break;
                case Operation.OperationType.jmp:
                    Console.Write("Input Key Value: ");
                    int.TryParse(Console.ReadLine(), out setValue);
                    break;
                case Operation.OperationType.None:
                case Operation.OperationType.halt:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var values = new ValueSet(operation, storageIndex, activeIndices, setValue);
            activeProgram.Add(values);
        }

        private static void RemoveOperation()
        {
            ShowOperations();
        }

        private static void ShowOperations()
        {
            foreach (var values in activeProgram)
            {
                Console.WriteLine(values.Operation.Function.ToString());
            }
        }

        private static void ShowAssistance()
        {
            Console.WriteLine("The Available Commands Are:");
            Console.WriteLine($"{nameof(Commands.run)} (Run the Current Program)");
            Console.WriteLine($"{nameof(Commands.quit)} (Quit the System)");
            Console.WriteLine($"{nameof(Commands.add)} (Add a new Operation to the Program)");
            Console.WriteLine($"{nameof(Commands.remove)} (Remove an Operation from the Program)");
            Console.WriteLine($"{nameof(Commands.program)} (Show All Operations in the Current Program)");
        }

        private static void UnrecognizedCommand()
        {
            Console.Write("Command Not Recognized, ");
            ShowAssistance();
        }
    }
}
