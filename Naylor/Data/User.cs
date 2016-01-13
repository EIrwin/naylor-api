using System;

namespace Naylor.Data
{
    public class User
    {
        public Guid Id { get; set; }

        public decimal HourlyRate { get; set; }

        public decimal Salary { get; set; }

        public decimal IntervalAmount { get; set; }
    }
}
