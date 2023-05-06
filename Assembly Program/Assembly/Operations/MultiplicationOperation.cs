using System.Linq;

namespace Assembly.Operations
{
    public class MultiplicationOperation : Operation
    {
        private readonly int baseIndex;
        private readonly int factorIndex;
        private readonly int productIndex;

        public MultiplicationOperation(int baseIndex, int factorIndex, int productIndex)
        {
            this.baseIndex = baseIndex;
            this.factorIndex = factorIndex;
            this.productIndex = productIndex;
        }

        public override void Operate()
        {
            bool[][] dataSet = SetDataSet();
            bool[] dataHolder = new bool[Register.BITS];

            bool[] factorValue = SetFactorValue();

            for (int i = 0; i < Register.BITS; i++)
            {
                bool b = factorValue[i];
                if (!b)
                    continue;
                dataHolder = LogicGates.Adder(dataHolder, dataSet[i]);
            }
            dataHolder = LogicGates.Xor(
                Assembly.activeRegister.Data[baseIndex][Register.BITS - 1],
                Assembly.activeRegister.Data[factorIndex][Register.BITS - 1])
                ? LogicGates.Invert(dataHolder) : dataHolder;
            Assembly.activeRegister.Data[productIndex] = dataHolder;
        }

        private bool[] SetFactorValue()
        {
            bool[] factorValue = Assembly.activeRegister.Data[factorIndex].ToArray(); // Using LINQ to get a separate instance of the array
            if (factorValue[factorValue.Length - 1])
                factorValue = LogicGates.Invert(factorValue);
            return factorValue;
        }

        private bool[][] SetDataSet()
        {
            bool[] dataHolder = Assembly.activeRegister.Data[baseIndex][Register.BITS - 1] ?
                LogicGates.Invert(Assembly.activeRegister.Data[baseIndex].ToArray()) :
                Assembly.activeRegister.Data[baseIndex].ToArray()
                    .Take(Register.BITS - 1)
                    .ToArray();
            bool[][] dataSet = Enumerable.Range(0, Register.BITS)
                .Select(d =>
                {
                    bool[] prevDataHolder = new bool[Register.BITS];
                    for (int i = 0; i < dataHolder.Length; i++)
                        prevDataHolder[i] = dataHolder[i];
                    dataHolder = ShiftOperation.Shift(dataHolder, true, false);
                    return prevDataHolder;
                })
                .ToArray();
            return dataSet;
        }
    }
}
