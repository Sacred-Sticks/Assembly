using System;

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

        private static void ConvertToBinary(int value, out bool[] binaryValue)
        {
            binaryValue = new bool[BITS];
            string binaryString = Convert.ToString(value, 2);

            for (int i = 0; i < binaryString.Length; i++)
            {
                if (binaryString[i] == '1')
                    binaryValue[i] = true;
            }
        }
    }
}
