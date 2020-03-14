namespace ConsoleInteractive.Question.Formats
{
    public class LongQuestion : BaseQuestion<long>
    {
        public LongQuestion() : base("What number", 0) { }
    }

    public class IntQuestion : BaseQuestion<int>
    {
        public IntQuestion() : base("What number", 0) { }
    }

    public class ULongQuestion : BaseQuestion<ulong>
    {
        public ULongQuestion() : base("What number", 0) { }
    }

    public class UIntQuestion : BaseQuestion<uint>
    {
        public UIntQuestion() : base("What number", 0) { }
    }

    public class DoubleQuestion : BaseQuestion<double>
    {
        public DoubleQuestion() : base("What number", 0) { }
    }

    public class FloatQuestion : BaseQuestion<float>
    {
        public FloatQuestion() : base("What number", 0) { }
    }

    public class DecimalQuestion : BaseQuestion<decimal>
    {
        public DecimalQuestion() : base("What number", 0) { }
    }
}