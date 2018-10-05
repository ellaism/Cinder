using System;

namespace EllaX.Core.Models
{
    public class TableRow
    {
        public int RowIndex { get; set; }

        public DateTime? RowCreated { get; set; }

        public DateTime? RowUpdated { get; set; }

        public bool IsNew()
        {
            return RowIndex == 0;
        }

        public void UpdateRowDates()
        {
            DateTime now = DateTime.UtcNow;

            if (RowCreated == null)
            {
                RowCreated = now;
            }

            RowUpdated = now;
        }
    }
}
