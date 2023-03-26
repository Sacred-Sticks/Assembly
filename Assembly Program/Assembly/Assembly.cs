using System;

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

        public static void Main()
        {
            bool running = true;
            var activeRegister = new Register(8);
            var storageRegister = new Register(64);
            
            IntroduceProgram();
            while (running)
            {
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
            Console.WriteLine("Running Program");
        }

        private static void AddOperation()
        {
            var operation = new Operation();
            operation.SetUpOperation();
            Console.WriteLine(operation.Function.ToString());
        }

        private static void RemoveOperation()
        {
            ShowOperations();
        }

        private static void ShowOperations()
        {
            Console.WriteLine("Print List Here:");
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
