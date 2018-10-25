using System;

namespace Demo1.BusinessEntity
{
    public class ExternalUserProfile
    {
        public string UserId { get; set; }
        public bool? AccountEnabled { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string Mail { get; set; }
        public string MailNickname { get; set; }
        public string LastName { get; set; }
        public string UserPrincipalName { get; set; }
        public string Connection { get; set; }
        public string Provider { get; set; }
        public dynamic AppMetadata { get; set; }
        public dynamic UserMetadata { get; set; }
        public bool? EmailVerified { get; set; }
        public string LoginsCount { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
