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
            bool[] original = Assembly.activeRegister.Data[readingIndexA];
            bool[] subtracted = LogicGates.Invert(Assembly.activeRegister.Data[readingIndexB]);
            bool[] binaryValue = LogicGates.Adder(original, subtracted);
            register.Data[writingIndex] = binaryValue;
        }
    }
}
