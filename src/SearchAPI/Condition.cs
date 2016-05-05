namespace SearchAPI
{
    public class Condition
    {
        public Operator Comporator { get; }
        public object Value { get; }

        public Condition(Operator comporator, object value)
        {
            Comporator = comporator;
            Value = value;
        }
    }
}