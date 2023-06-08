using System;
namespace WeFix.Models
{
    public class CompletedAppointment
    {
        public string CustomerName { get; set; }
        public string VehicleReg { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public List<UsedPart> UsedParts { get; set; }

        // Other properties and methods for the completed appointment

        public class UsedPart
        {
            public string PartName { get; set; }
            public int QuantityUsed { get; set; }

            // Other properties for the used part
        }
    }
}

