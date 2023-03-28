namespace Assembly.Operations
{
    public class HaltOperation : Operation
    {
        public override void Operate()
        {
            Assembly.EndProgram();
        }
    }
}
