namespace Assembly
{
    public struct ValueSet
    {
        public ValueSet(Operation operation, int storageIndex, int[] activeIndices, int setValue)
        {
            Operation = operation;
            StorageIndex = storageIndex;
            ActiveIndices = activeIndices;
            SetValue = setValue;
        }
        
        public Operation Operation { get; }
        public int StorageIndex { get; }
        public int[] ActiveIndices { get; }
        public int SetValue { get; }
    }
}
