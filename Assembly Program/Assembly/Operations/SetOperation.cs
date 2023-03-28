namespace Assembly.Operations
{
    public class SetOperation : Operation
    {
        public SetOperation(int value, int setToIndex)
        {
            this.value = value;
            this.setToIndex = setToIndex;
        }
        
        private readonly int value;
        private readonly int setToIndex;
        
        public override void Operate()
        {
            Register.ConvertToBinary(value, out bool[] binaryValue);
            Assembly.activeRegister.Data[setToIndex] = binaryValue;
        }
    }
}
