using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.DTO
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }
    }
}
