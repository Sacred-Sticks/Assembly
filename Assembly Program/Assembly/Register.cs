using System;
using System.Collections.Generic;

namespace Assembly
{
    public class Register
    {
        public Register(int size)
        {
            maxValue = (int)Math.Pow(2, BITS) - 1;
            
            Data = new bool[size][];
            for (int i = 0; i < size; i++)
                Data[i] = new bool[BITS];
        }
        
        public bool[][] Data { get; private set; }

        private readonly int maxValue;
        
        private const int BITS = 8;

        public void SetData(int index, int value)
        {
            if (value > maxValue)
                throw new ArgumentOutOfRangeException(nameof(value));
            if (index > Data.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            
            ConvertToBinary(value, out bool[] binaryValue);
            Data[index] = binaryValue;
        }

        public static void ConvertToBinary(int value, out bool[] binaryValue)
        {
            binaryValue = new bool[BITS];
            string binaryString = Convert.ToString(value, 2);
            
            string binary = "";
            int count = binaryString.Length;
            while (count < BITS)
            {
                binary += '0';
                count++;
            }
            binary += binaryString;

            for (int i = 0; i < BITS; i++)
            {;
                if (binary[i] == '1')
                    binaryValue[i] = true;
            }
        }

        public static void ConvertToInteger(bool[] binaryValue, out int value)
        {
            string binaryString = "";
            for(int i = 0; i < BITS; i++)
            {
                if (binaryValue[i])
                {
                    binaryString += 1.ToString();
                    continue;
                }
                binaryString += 0.ToString();
            }
            value = Convert.ToInt32(binaryString, 2);
        }
    }
}
