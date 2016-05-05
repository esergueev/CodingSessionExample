using System;

namespace SearchAPI
{
    public class Ordering
    {
        public string Column { get; }
        public OrderingDirection Direction { get; }

        public Ordering(string column, OrderingDirection direction)
        {
            if (string.IsNullOrEmpty(column))
            {
                throw new ArgumentNullException(nameof(column));
            }

            Column = column;
            Direction = direction;
        }
    }
}