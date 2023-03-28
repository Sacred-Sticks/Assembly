namespace Assembly.Operations
{
    public class DecreaseOperation : Operation
    {
        public DecreaseOperation(int readingIndex)
        {
            this.readingIndex = readingIndex;
        }
        
        private readonly int readingIndex;
        
        public override void Operate()
        {
            bool[] one = new bool[Register.BITS];
            one[0] = true;
            one = LogicGates.Invert(one);
            bool[] binaryValue = Assembly.activeRegister.Data[readingIndex];
            binaryValue = LogicGates.Adder(binaryValue, one);
            Assembly.activeRegister.Data[readingIndex] = binaryValue;
        }
    }
}
