namespace Assembly.Operations
{
    public class SaveOperation : Operation
    {
        public SaveOperation(int saveFromIndex, int saveToIndex)
        {
            this.saveFromIndex = saveFromIndex;
            this.saveToIndex = saveToIndex;
        }
        
        private readonly int saveFromIndex;
        private readonly int saveToIndex;
        
        public override void Operate()
        {
            Assembly.storageRegister.Data[saveToIndex] = Assembly.activeRegister.Data[saveFromIndex];
        }
    }
}
