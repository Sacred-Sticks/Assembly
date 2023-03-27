using System;
using System.Collections.Generic;

namespace Assembly
{
    public class Register
    {
        public Register(int size)
        {
            Data = new bool[size][];
            for (int i = 0; i < size; i++)
                Data[i] = new bool[BITS];
        }
        
        public bool[][] Data { get; }

        public const int BITS = 8;

        public static void ConvertToBinary(int value, out bool[] binaryValue)
        {
            int currentValue = value;
            if (currentValue < 0)
                currentValue *= -1;
            binaryValue = new bool[BITS];
            for (int i = 0; i < BITS - 1; i++)
            {
                binaryValue[i] = currentValue % 2 == 1;
                currentValue /= 2;
            }
            if (value < 0)
                binaryValue = LogicGates.Invert(binaryValue);
        }

        public static void ConvertToInteger(bool[] binaryValue, out int value)
        {
            value = 0;
            int multiplier = 1;
            bool isNegative = binaryValue[BITS - 1];
            if (binaryValue[BITS - 1])
                binaryValue = LogicGates.Invert(binaryValue);
            for (int i = 0; i < BITS - 1; i++)
            {
                if (binaryValue[i])
                    value += multiplier;
                multiplier *= 2;
            }
            if (isNegative)
                value *= -1;
        }
    }
}
