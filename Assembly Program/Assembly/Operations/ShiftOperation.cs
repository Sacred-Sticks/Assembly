using System.Collections.Generic;
using System.Linq;

namespace Assembly.Operations
{
    public class ShiftOperation : Operation
    {
        public ShiftOperation(int shiftIndex, bool shiftLeft = false, bool shiftRight = false)
        {
            this.shiftIndex = shiftIndex;
            this.shiftLeft = shiftLeft;
            this.shiftRight = shiftRight;
        }

        private readonly int shiftIndex;
        private readonly bool shiftLeft;
        private readonly bool shiftRight;

        public override void Operate()
        {
            Assembly.activeRegister.Data[shiftIndex] = Shift(Assembly.activeRegister.Data[shiftIndex], shiftLeft, shiftRight);
        }

        public static bool[] Shift(IEnumerable<bool> data, bool shiftLeft, bool shiftRight)
        {
            return (System.Array.Empty<bool>())
                .Concat(Enumerable.Repeat(false, shiftLeft ? 1 : 0))
                .Concat(data
                    .Skip(shiftRight ? 1 : 0)
                    .Take(Register.BITS - 1))
                .Concat(Enumerable.Repeat(false, shiftRight ? 1 : 0))
                .Take(Register.BITS)
                .ToArray();
        }
    }
}
