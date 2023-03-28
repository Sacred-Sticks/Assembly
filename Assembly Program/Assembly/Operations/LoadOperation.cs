namespace Assembly.Operations
{
    public class LoadOperation : Operation
    {
        public LoadOperation(int loadFromIndex, int loadToIndex)
        {
            this.loadFromIndex = loadFromIndex;
            this.loadToIndex = loadToIndex;
        }
        
        private readonly int loadFromIndex;
        private readonly int loadToIndex;
        
        public override void Operate()
        {
            Assembly.activeRegister.Data[loadToIndex] = Assembly.storageRegister.Data[loadFromIndex];
        }
    }
}
