using MHC_AFMS.Models;

namespace MHC_AFMS.Helpers
{
    // Simple class to hold data for the view
    public class AppointmentSlot
    {
        public TimeSpan Time { get; set; }      // The slot time (e.g., 09:15)
        public bool IsAvailable { get; set; }   // Can it be booked?
    }

    public static class SlotGenerator
    {
        // LOGIC: Generate 15-minute intervals based on start/end hours
        public static List<AppointmentSlot> GenerateSlots(int startHour, int endHour, List<TimeSpan> bookedTimes)
        {
            var slots = new List<AppointmentSlot>();

            // Define Start and End based on the doctor's schedule
            TimeSpan current = new TimeSpan(startHour, 0, 0);
            TimeSpan end = new TimeSpan(endHour, 0, 0);

            // Loop until we reach the end of the shift
            while (current < end)
            {
                // Check if this specific time is already in the database list
                bool isBooked = bookedTimes.Contains(current);

                slots.Add(new AppointmentSlot
                {
                    Time = current,
                    IsAvailable = !isBooked // If booked, availability is FALSE
                });

                // Add 15 minutes for the next loop
                current = current.Add(new TimeSpan(0, 15, 0));
            }

            return slots;
        }
    }
}