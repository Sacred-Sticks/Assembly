using System;

namespace Assembly
{
    public class Operation
    {
        public enum OperationType
        {
            None,
            add, // A = Add(B, C) A = B + C
            sub, // A = Subtract(B, C) A = B - C
            set, // A = LoadImmediate(value) A = value
            inc, // A = Increase(B) A = B + 1
            dec, // A = Decrease(B) A = B - 1
            pow, // A = Power(B, C) A = B^C
            jmp, // Jump(A) Load operation at Index A
            jiz, // JumpIfZero(A, B) Load operation at Index A if B is 0
            str, // Store(A, B) Store Data from Index A in Active Register at Index B of Storage Register
            load, // Load(A, B) Load Data from Index A of Storage Register into Index B of Active Register
            halt, // Ends the program
        }

        public OperationType Function { get; private set; }

        private int indexA;
        private int indexB;
        private int value;

        public void SetUpOperation()
        {
            Console.WriteLine("The Operation Types Are:");
            Console.WriteLine($"{nameof(OperationType.add)} (Add A + B)");
            Console.WriteLine($"{nameof(OperationType.sub)} (Subtract B - A)");
            Console.WriteLine($"{nameof(OperationType.set)} (Set A to value)");
            Console.WriteLine($"{nameof(OperationType.inc)} (Increase A by 1)");
            Console.WriteLine($"{nameof(OperationType.dec)} (Decrease A by 1)");
            Console.WriteLine($"{nameof(OperationType.pow)} (Raise A to the Power B)");
            Console.WriteLine($"{nameof(OperationType.jmp)} (Jump to the Operation at Index A)");
            Console.WriteLine($"{nameof(OperationType.jiz)} (Jump to the Operation at Index A if B is 0)");
            Console.WriteLine($"{nameof(OperationType.str)} (Store A at Index B of Storage Register)");
            Console.WriteLine($"{nameof(OperationType.load)} (Load A from Index B of Storage Register");
            Console.WriteLine($"{nameof(OperationType.halt)} (Ends the Program");
            
            AssignFunction();
        }

        private void AssignFunction()
        {
            Console.WriteLine();
            Console.WriteLine("Enter The Operation Being Added:");
            string userInput = Console.ReadLine()?.ToLower();

            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("ERROR: Missing Operation Input");
                return;
            }
            
            switch (userInput)
            {
                case nameof(OperationType.add):
                    Function = OperationType.add;
                    break;
                case nameof(OperationType.sub):
                    Function = OperationType.sub;
                    break;
                case nameof(OperationType.set):
                    Function = OperationType.set;
                    break;
                case nameof(OperationType.inc):
                    Function = OperationType.inc;
                    break;
                case nameof(OperationType.dec):
                    Function = OperationType.dec;
                    break;
                case nameof(OperationType.pow):
                    Function = OperationType.pow;
                    break;
                case nameof(OperationType.jmp):
                    Function = OperationType.jmp;
                    break;
                case nameof(OperationType.jiz):
                    Function = OperationType.jiz;
                    break;
                case nameof(OperationType.str):
                    Function = OperationType.str;
                    break;
                case nameof(OperationType.load):
                    Function = OperationType.load;
                    break;
                case nameof(OperationType.halt):
                    Function = OperationType.halt;
                    break;
                default:
                    Console.WriteLine("Operation Not Found");
                    break;
            }
        }
    }
}
