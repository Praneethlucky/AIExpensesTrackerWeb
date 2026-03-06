using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain.Entities
{
    public class DeviceSession
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string DeviceId { get; set; }

        public string DeviceName { get; set; }

        public string RefreshToken { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
