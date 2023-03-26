using System;
using System.Collections.Generic;

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
            jmp, // Jump(A) Load operation at Index A
            jiz, // JumpIfZero(A, B) Load operation at Index A if B is 0
            str, // Store(A, B) Store Data from Index A in Active Register at Index B of Storage Register
            load, // Load(A, B) Load Data from Index A of Storage Register into Index B of Active Register
            halt, // Ends the program
        }

        public OperationType Function { get; private set; }

        public void SetUpOperation()
        {
            Console.WriteLine("The Operation Types Are:");
            Console.WriteLine($"{nameof(OperationType.add)} (Add A + B)");
            Console.WriteLine($"{nameof(OperationType.sub)} (Subtract B - A)");
            Console.WriteLine($"{nameof(OperationType.set)} (Set A to value)");
            Console.WriteLine($"{nameof(OperationType.inc)} (Increase A by 1)");
            Console.WriteLine($"{nameof(OperationType.dec)} (Decrease A by 1)");
            Console.WriteLine($"{nameof(OperationType.jmp)} (Jump to the Operation at Index A)");
            Console.WriteLine($"{nameof(OperationType.jiz)} (Jump to the Operation at Index A if B is 0)");
            Console.WriteLine($"{nameof(OperationType.str)} (Store A at Index B of Storage Register)");
            Console.WriteLine($"{nameof(OperationType.load)} (Load A from Index B of Storage Register");
            Console.WriteLine($"{nameof(OperationType.halt)} (Ends the Program");
            
            AssignFunction();
        }

        public void TriggerOperation(int storageIndex, int[] activeIndices, int setValue, out bool continueRunning)
        {
            continueRunning = true;
            switch (Function)
            {
                case OperationType.add:
                    Add(Assembly.activeRegister, storageIndex, activeIndices[0], activeIndices[1]);
                    break;
                case OperationType.sub:
                    Subtract(Assembly.activeRegister, storageIndex, activeIndices[0], activeIndices[1]);
                    break;
                case OperationType.inc:
                    Increase(Assembly.activeRegister, storageIndex, activeIndices[0]);
                    break;
                case OperationType.dec:
                    Decrease(Assembly.activeRegister, storageIndex, activeIndices[0]);
                    break;
                case OperationType.str:
                    Store(storageIndex, activeIndices[0]);
                    break;
                case OperationType.load:
                    Load(storageIndex, activeIndices[0]);
                    break;
                case OperationType.set:
                    Set(Assembly.activeRegister, storageIndex, setValue);
                    break;
                case OperationType.jiz:
                    JumpIfZero(Assembly.activeRegister, storageIndex, setValue);
                    break;
                case OperationType.jmp:
                    Jump(setValue);
                    break;
                case OperationType.halt:
                    continueRunning = false;
                    return;
                case OperationType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        private static void ReadSource(Register target, int readingIndex, out int output)
        {
            Register.ConvertToInteger(target.Data[readingIndex], out int value);
            output = value;
        }

        private static void Add(Register target, int writingIndex, int readingIndexA, int readingIndexB)
        {
            ReadSource(target, readingIndexA, out int valueA);
            ReadSource(target, readingIndexB, out int valueB);
            int value = valueA + valueB;
            Register.ConvertToBinary(value, out bool[] binaryValue);
            target.Data[writingIndex] = binaryValue;
        }
        
        private static void Subtract(Register target, int writingIndex, int readingIndexA, int readingIndexB)
        {
            ReadSource(target, readingIndexA, out int valueA);
            ReadSource(target, readingIndexB, out int valueB);
            int value = valueA + valueB;
            Register.ConvertToBinary(value, out bool[] binaryValue);
            target.Data[writingIndex] = binaryValue;
        }

        private static void Set(Register target, int writingIndex, int value)
        {
            Register.ConvertToBinary(value, out bool[] binaryValue);
            target.Data[writingIndex] = binaryValue;
        }

        private static void Increase(Register target, int writingIndex, int readingIndex)
        {
            ReadSource(target, readingIndex, out int value);
            value++;
            Register.ConvertToBinary(value, out bool[] binaryValue);
            target.Data[writingIndex] = binaryValue;
        }

        private static void Decrease(Register target, int writingIndex, int readingIndex)
        {
            ReadSource(target, readingIndex, out int value);
            value--;
            Register.ConvertToBinary(value, out bool[] binaryValue);
            target.Data[writingIndex] = binaryValue;
        }

        private static void Jump(int jumpIndex)
        {
            Assembly.operationIndex = jumpIndex;
        }

        private static void JumpIfZero(Register target, int readingIndex, int jumpIndex)
        {
            ReadSource(target, readingIndex, out int output);
            if (output == 0)
                Jump(jumpIndex);
        }

        private static void Store(int writingIndex, int readingIndex)
        {
            Assembly.storageRegister.Data[writingIndex] = Assembly.activeRegister.Data[readingIndex];
        }

        private static void Load(int writingIndex, int readingIndex)
        {
            Assembly.activeRegister.Data[writingIndex] = Assembly.storageRegister.Data[readingIndex];
        }
    }
}
