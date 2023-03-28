using System.Linq;

namespace Assembly.Operations
{
    public class JumpIfZeroOperation : Operation
    {
        public JumpIfZeroOperation(int jumpToIndex, int zeroCheckIndex)
        {
            this.jumpToIndex = jumpToIndex;
            this.zeroCheckIndex = zeroCheckIndex;
        }
        
        private readonly int jumpToIndex;
        private readonly int zeroCheckIndex;
        
        public override void Operate()
        {
            if (Assembly.activeRegister.Data[zeroCheckIndex].Any(boolean => boolean))
                return;
            Assembly.programIndex = jumpToIndex;
        }
    }
}
