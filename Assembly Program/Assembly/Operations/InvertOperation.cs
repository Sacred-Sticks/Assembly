namespace Assembly.Operations
{
    public class InvertOperation : Operation
    {
        public InvertOperation(int readingIndex)
        {
            this.readingIndex = readingIndex;
        }
        
        private readonly int readingIndex;
        
        public override void Operate()
        {
            bool[] binaryValue = Assembly.activeRegister.Data[readingIndex];
            binaryValue = LogicGates.Invert(binaryValue);
            Assembly.activeRegister.Data[readingIndex] = binaryValue;
        }
    }
}
