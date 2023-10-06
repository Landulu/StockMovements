using System;
using System.Collections.Generic;

namespace FnacDarty.JobInterview.Stock
{
    public class Movement
    {
        public Movement(MovementType type, string label, IList<ProductMovement> productMovements, DateTime date)
        {
            Type = type;
            Label = label;
            ProductMovements = productMovements;
            Date = date;
        }

        public MovementType Type { get; private set; }
        public string Label { get; private set; }
        public IList<ProductMovement> ProductMovements { get; private set; }
        public DateTime Date { get; private set; }
    }
}