using System.Collections.Generic;
using System.Linq;

namespace Assembly.Operations
{
    public class SubtractOperation : Operation
    {
        public SubtractOperation(int readingIndexA, int readingIndexB, int writingIndex)
        {
            this.readingIndexA = readingIndexA;
            this.readingIndexB = readingIndexB;
            this.writingIndex = writingIndex;
        }
        
        private readonly int readingIndexA;
        private readonly int readingIndexB;
        private readonly int writingIndex;
        
        private readonly Register register = Assembly.activeRegister;
        
        public override void Operate()
        {
            bool[] binaryValue = Subtract(Assembly.activeRegister.Data[readingIndexA], Assembly.activeRegister.Data[readingIndexB]);
            register.Data[writingIndex] = binaryValue;
        }

        public static bool[] Subtract(IEnumerable<bool> a, IEnumerable<bool> b)
        {
            bool[] subtracted = LogicGates.Invert(b.ToArray());
            bool[] binaryValue = LogicGates.Adder(a.ToArray(), subtracted);
            return binaryValue;
        }
    }
}
