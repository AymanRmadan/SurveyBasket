﻿using Microsoft.EntityFrameworkCore;

namespace SurveyBasket.Entities
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedOn { get; set; }
        public bool IsExpire => DateTime.UtcNow >= ExpiresOn;
        public bool IsActive => RevokedOn is null && !IsExpire;
    }
}
