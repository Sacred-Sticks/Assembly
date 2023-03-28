using System;
using System.Linq;

namespace Assembly
{
    public class Operation
    {
        public enum OperationType
        {
            none,
            add, // A = Add(B, C) A = B + C
            sub, // A = Subtract(B, C) A = B - C
            set, // A = LoadImmediate(value) A = value
            inc, // A = Increase(B) A = B + 1
            dec, // A = Decrease(B) A = B - 1
            inv,
            jump, // Jump(A) Load operation at Index A
            jiz, // JumpIfZero(A, B) Load operation at Index A if B is 0
            save, // Store(A, B) Store Data from Index A in Active Register at Index B of Storage Register
            load, // Load(A, B) Load Data from Index A of Storage Register into Index B of Active Register
            halt, // Ends the program
            stop, // Stop adding new operations
        }

        public OperationType Function { get; private set; }

        public void TriggerOperation(int storageIndex, int[] activeIndices, int setValue, out bool continueRunning)
        {
            continueRunning = true;
            if (storageIndex < 0 || storageIndex > Assembly.storageRegisterSize)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(storageIndex), 
                    storageIndex, 
                    $"{nameof(storageIndex)} must be between 0 and {Assembly.storageRegisterSize}"
                    );
            }
            switch (Function)
            {
                case OperationType.add:
                    Add(activeIndices[0], activeIndices[1], activeIndices[2]);
                    break;
                case OperationType.sub:
                    Subtract(activeIndices[0], activeIndices[1], activeIndices[2]);
                    break;
                case OperationType.inc:
                    Increase(activeIndices[0]);
                    break;
                case OperationType.dec:
                    Decrease(activeIndices[0]);
                    break;
                case OperationType.inv:
                    Invert(activeIndices[0]);
                    break;
                case OperationType.save:
                    Store(storageIndex, activeIndices[0]);
                    break;
                case OperationType.load:
                    Load(storageIndex, activeIndices[0]);
                    break;
                case OperationType.set:
                    Set(activeIndices[0], setValue);
                    break;
                case OperationType.jiz:
                    JumpIfZero(activeIndices[0], setValue);
                    break;
                case OperationType.jump:
                    Jump(setValue);
                    break;
                case OperationType.none:
                case OperationType.halt:
                case OperationType.stop:
                    continueRunning = false;
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AssignFunction(out bool newOperation)
        {
            Console.WriteLine();
            Console.WriteLine("Enter The Operation Being Added:");
            string userInput = Console.ReadLine()?.ToLower();

            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("ERROR: Missing Operation Input");
                newOperation = false;
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
                case nameof(OperationType.inv):
                    Function = OperationType.inv;
                    break;
                case nameof(OperationType.jump):
                    Function = OperationType.jump;
                    break;
                case nameof(OperationType.jiz):
                    Function = OperationType.jiz;
                    break;
                case nameof(OperationType.save):
                    Function = OperationType.save;
                    break;
                case nameof(OperationType.load):
                    Function = OperationType.load;
                    break;
                case nameof(OperationType.halt):
                    Function = OperationType.halt;
                    break;
                case nameof(OperationType.stop):
                    Function = OperationType.halt;
                    newOperation = false;
                    return;
                default:
                    Console.WriteLine("Operation Not Found");
                    break;
            }
            newOperation = true;
        }

        private static void Add(int writingIndex, int readingIndexA, int readingIndexB)
        {
            bool[] original = Assembly.activeRegister.Data[readingIndexA];
            bool[] addition = Assembly.activeRegister.Data[readingIndexB];
            bool[] binaryValue = LogicGates.Adder(original, addition);
            Assembly.activeRegister.Data[writingIndex] = binaryValue;
        }

        private static void Subtract(int writingIndex, int readingIndexA, int readingIndexB)
        {
            bool[] original = Assembly.activeRegister.Data[readingIndexA];
            bool[] subtracted = LogicGates.Invert(Assembly.activeRegister.Data[readingIndexB]);
            bool[] binaryValue = LogicGates.Adder(original, subtracted);
            Assembly.activeRegister.Data[writingIndex] = binaryValue;
        }

        private static void Set(int writingIndex, int value)
        {
            Register.ConvertToBinary(value, out bool[] binaryValue);
            Assembly.activeRegister.Data[writingIndex] = binaryValue;
        }

        private static void Increase(int writingIndex)
        {
            bool[] one = new bool[Register.BITS];
            one[0] = true;
            bool[] binaryValue = Assembly.activeRegister.Data[writingIndex];
            binaryValue = LogicGates.Adder(binaryValue, one);
            Assembly.activeRegister.Data[writingIndex] = binaryValue;
        }

        private static void Decrease(int writingIndex)
        {
            bool[] one = new bool[Register.BITS];
            one[0] = true;
            one = LogicGates.Invert(one);
            bool[] binaryValue = Assembly.activeRegister.Data[writingIndex];
            binaryValue = LogicGates.Adder(binaryValue, one);
            Assembly.activeRegister.Data[writingIndex] = binaryValue;
        }

        private static void Invert(int writingIndex)
        {
            bool[] binaryValue = Assembly.activeRegister.Data[writingIndex];
            binaryValue = LogicGates.Invert(binaryValue);
            Assembly.activeRegister.Data[writingIndex] = binaryValue;
        }
        
        private static void Jump(int jumpIndex)
        {
            Assembly.operationIndex = jumpIndex;
        }

        private static void JumpIfZero(int readingIndex, int jumpIndex)
        {
            if (Assembly.activeRegister.Data[readingIndex].Any(boolean => boolean))
                return;
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
