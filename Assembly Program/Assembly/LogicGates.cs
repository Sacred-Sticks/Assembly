namespace Assembly
{
    public static class LogicGates
    {
        public static bool Not(bool input)
        {
            return !input;
        }

        public static bool Or(bool a, bool b)
        {
            return a || b;
        }

        public static bool And(bool a, bool b)
        {
            return Not(Or(Not(a), Not(b)));
        }

        public static bool Nand(bool a, bool b)
        {
            return Not(Or(a, b));
        }

        public static bool Xor(bool a, bool b)
        {
            bool notOrAB = Not(Or(a, b));
            return Or(Not(Or(a, notOrAB)), Not(Or(b, notOrAB)));
        }

        public static bool[] Adder(bool[] a, bool[] b)
        {
            bool[] result = new bool[Register.BITS];
            bool[] carryValues = new bool[Register.BITS];
            bool carryValue = false;
            for (int i = 0; i < Register.BITS - 1; i++)
            {
                if (And(a[i], b[i]))
                    carryValue = true;
                if (Nand(a[i], b[i]))
                    carryValue = false;
                carryValues[i + 1] = carryValue;
            }
            carryValues[0] = false;
            for (int i = 0; i < Register.BITS; i++)
            {
                result[i] = Xor(Xor(a[i], b[i]), carryValues[i]);
            }
            return result;
        }

        public static bool[] Invert(bool[] a)
        {
            bool[] one = new bool[Register.BITS];
            one[0] = true;
            for (int i = 0; i < Register.BITS; i++)
            {
                a[i] = Not(a[i]);
            }
            a = Adder(a, one);
            return a;
        }
    }
}
