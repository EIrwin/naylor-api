using System;

namespace Naylor.Gateway.Models
{
    public class CreateUserRequest
    {
        public decimal HourlyRate { get; set; }

        public decimal Salary { get; set; }
    }

    public class CreateMeetingRequest
    {
        
    }

    public class GetMeetingByIdRequest
    {
        public Guid Id { get; set; }
    }

    public class JoinMeetingRequest
    {
        public Guid UserId { get; set; }

        public Guid MeetingId { get; set; }
    }

    public class LeaveMeetingRequest
    {
        public Guid UserId { get; set; }

        public Guid MeetingId { get; set; }
    }

    public class StartMeetingRequest
    {
        public Guid MeetingId { get; set; }
    }

    public class EndMeetingRequest
    {
        public Guid MeetingId { get; set; }
    }
}
