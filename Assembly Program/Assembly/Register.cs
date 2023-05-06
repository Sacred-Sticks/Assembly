using System;
using System.Collections.Generic;
using System.Linq;

namespace Assembly
{
    public class Register
    {
        public Register(int size)
        {
            minValue = -1;
            for (int i = 1; i < BITS; i++)
            {
                minValue *= 2;
            }
            maxValue = -minValue - 1;
            Data = new bool[size][];
            for (int i = 0; i < size; i++)
                Data[i] = new bool[BITS];
        }
        
        public bool[][] Data { get; }

        public const int BITS = 8;
        public static int maxValue { get; private set; }
        public static int minValue { get; private set; }

        public static void ConvertToBinary(int value, out bool[] binaryValue)
        {
            if (value < minValue || value > maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, $"{nameof(value)} must be between {minValue} and {maxValue}");
            }

            int currentValue = value < 0 ? value - minValue : value;
            binaryValue = Enumerable.Range(0, BITS)
                .Select (i => {
                bool bit = currentValue % 2 == 1;
                currentValue /= 2;
                return bit;
            })
                .ToArray();
            binaryValue[BITS - 1] = value < 0;
        }

        public static void ConvertToInteger(IEnumerable<bool> binaryValue, out int value)
        {
            bool[] binary = binaryValue as bool[] ?? binaryValue.ToArray();
            bool isNegative = binary
                .Last();

            value = binary
                .Take(binary.Count() - 1)
                .Select((b, i) => (b ? 1 : 0) * (int)Math.Pow(2, i)).Sum();

            if (isNegative)
                value += minValue;
        }
    }
}
