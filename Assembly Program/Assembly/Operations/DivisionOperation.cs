using System.Collections.Generic;
using System.Linq;

namespace Assembly.Operations
{
    public class DivisionOperation : Operation
    {
        private readonly int numeratorIndex;
        private readonly int denominatorIndex;
        private readonly int writeToIndex;

        public DivisionOperation(int numeratorIndex, int denominatorIndex, int writeToIndex)
        {
            this.numeratorIndex = numeratorIndex;
            this.denominatorIndex = denominatorIndex;
            this.writeToIndex = writeToIndex;
        }

        public override void Operate()
        {
            bool[] numerator = (Assembly.activeRegister.Data[numeratorIndex][Register.BITS - 1] 
                    ? LogicGates.Invert(Assembly.activeRegister.Data[numeratorIndex].ToArray()) 
                    : Assembly.activeRegister.Data[numeratorIndex].ToArray())
                .Reverse()
                .ToArray();
            bool[] denominator = (Assembly.activeRegister.Data[denominatorIndex][Register.BITS - 1]
                ? LogicGates.Invert(Assembly.activeRegister.Data[denominatorIndex].ToArray()) : Assembly.activeRegister.Data[denominatorIndex])
                .ToArray();

            bool[] remainder = new bool[Register.BITS];
            bool[] result = new bool[Register.BITS];

            for (int i = 0; i < Register.BITS; i++)
            {
                remainder = ShiftOperation.Shift(remainder, true, false);
                remainder[0] = numerator[i];
                bool[] subtracted = SubtractOperation.Subtract(remainder, denominator);
                if (subtracted[Register.BITS - 1])
                {
                    result[i] = false;
                    continue;
                }
                result[i] = true;
                remainder = subtracted;
            }
            result = LogicGates.Xor(Assembly.activeRegister.Data[numeratorIndex][Register.BITS - 1], Assembly.activeRegister.Data[denominatorIndex][Register.BITS - 1])
                ? LogicGates.Invert(result
                    .Reverse()
                    .ToArray())
                : result
                    .Reverse()
                    .ToArray();
            Assembly.activeRegister.Data[writeToIndex] = result;
        }
    }
}
