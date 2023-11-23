using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phoneBook.Classes
{
    public class Call
    {
        public DateTime CallTime { get; }
        public CallStatus Status { get; set; }


        public Call(DateTime callTime, CallStatus status)
        {
            CallTime = callTime;
            Status = status;
        }
    }

    public enum CallStatus
    {
        InProgress,
        Missed,
        Completed
    }
}
