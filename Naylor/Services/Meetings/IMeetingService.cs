using System.Threading.Tasks;
using Naylor.Data;

namespace Naylor.Services
{
    public interface IMeetingService
    {
        Task<Meeting> CreateMeeting(CreateMeeting createMeeting);
        Task<Meeting> GetMeetingById(GetMeetingById getMeetingById);
        Task<Meeting> StartMeeting(StartMeeting startMeeting);
        Task<Meeting> EndMeeting(EndMeeting endMeeting);
        Task<Meeting> JoinMeeting(JoinMeeting joinMeeting);
        Task<Meeting> LeaveMeeting(LeaveMeeting leaveMeeting);
        Task<Meeting> UpdateTotal(UpdateTotal updateTotal);
    }
}
