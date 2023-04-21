namespace Assembly.Operations
{
    public class JumpOperation : Operation
    {
        public JumpOperation(int jumpToIndex)
        {
            this.jumpToIndex = jumpToIndex;
        }
        
        private readonly int jumpToIndex;
        
        public override void Operate()
        {
            Assembly.ProgramIndex = jumpToIndex;
        }
    }
}
