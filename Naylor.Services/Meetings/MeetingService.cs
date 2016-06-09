using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Naylor.Data;

namespace Naylor.Services.Meetings
{
    public class MeetingService:IMeetingService
    {
        private IRepository<Meeting> _meetingRepository;
        private IUserService _userService;
        public MeetingService(IRepository<Meeting> meetingRepository,IUserService userService)
        {
            _meetingRepository = meetingRepository;
            _userService = userService;
        }
        public async Task<Meeting> CreateMeeting(CreateMeeting createMeeting)
        {
            try
            {

                Meeting meeting = new Meeting();
                meeting = _meetingRepository.Save(meeting);
                return await Task.FromResult(meeting);

            }
            catch (Exception exception)
            {
                Console.Out.WriteLine(exception.Message);
            }
            return await Task.FromResult<Meeting>(null);
        }

        public async Task<Meeting> GetMeetingById(GetMeetingById getMeetingById)
        {
            try
            {
                var meeting = _meetingRepository.AsQueryable().FirstOrDefault(p => p.Id == getMeetingById.Id);

                //update total if meeting has started and not finished
                if (meeting.Active)
                {
                    meeting = await UpdateTotal(new UpdateTotal()
                        {
                            Meeting = meeting
                        });

                }

                return await Task.FromResult(meeting);

            }
            catch (Exception exception)
            {
                Console.Out.WriteLine(exception.Message);
            }
            return await Task.FromResult<Meeting>(null);
        }

        public async Task<Meeting> StartMeeting(StartMeeting startMeeting)
        {
            try
            {
                var meeting = _meetingRepository.AsQueryable().FirstOrDefault(p => p.Id == startMeeting.MeetingId);
                meeting.StartUtc = DateTime.UtcNow;
                meeting.Active = true;
                meeting = _meetingRepository.Save(meeting);
                return await Task.FromResult(meeting);

            }
            catch (Exception exception)
            {
                Console.Out.WriteLine(exception.Message);
            }
            return await Task.FromResult<Meeting>(null);
        }

        public async Task<Meeting> EndMeeting(EndMeeting endMeeting)
        {
            try
            {
                var meeting = _meetingRepository.AsQueryable().FirstOrDefault(p => p.Id == endMeeting.MeetingId);
                meeting.EndUtc = DateTime.Now.ToUniversalTime();
                meeting.Active = false;
                meeting = _meetingRepository.Save(meeting);
                return await Task.FromResult(meeting);

            }
            catch (Exception exception)
            {
                Console.Out.WriteLine(exception.Message);
            }
            return await Task.FromResult<Meeting>(null);
        }

        public async Task<Meeting> JoinMeeting(JoinMeeting joinMeeting)
        {
            try
            {
                var user = await _userService.GetUserById(new GetUserById() {Id = joinMeeting.UserId});

                var meeting = _meetingRepository.AsQueryable().First(p => p.Id == joinMeeting.MeetingId);

                meeting.Attendees.Add(user);

                meeting = _meetingRepository.Save(meeting);

                return await Task.FromResult(meeting);

            }
            catch (Exception exception)
            {
                Console.Out.WriteLine(exception.Message);
            }
            return await Task.FromResult<Meeting>(null);
        }

        public async Task<Meeting> LeaveMeeting(LeaveMeeting leaveMeeting)
        {
            try
            {
                var user = await _userService.GetUserById(new GetUserById() { Id = leaveMeeting.UserId });

                var meeting = _meetingRepository.AsQueryable().FirstOrDefault(p => p.Id == leaveMeeting.MeetingId);

                var attendees = new List<User>();
                foreach (User attendee in meeting.Attendees)
                {
                    if (attendee.Id != user.Id)
                        attendees.Add(new User()
                            {
                                Id = attendee.Id, HourlyRate = attendee.HourlyRate, Salary = attendee.Salary
                            });
                }
                meeting.Attendees = attendees;
                meeting = _meetingRepository.Save(meeting);

                return await Task.FromResult(meeting);

            }
            catch (Exception exception)
            {
                Console.Out.WriteLine(exception.Message);
            }
            return await Task.FromResult<Meeting>(null);
        }

        public async Task<Meeting> UpdateTotal(UpdateTotal updateTotal)
        {
            decimal total = updateTotal.Meeting.Total;
            double minutesPassed = (DateTime.Now.ToUniversalTime() - updateTotal.Meeting.StartUtc).TotalMinutes;
            double hourlyPercent = minutesPassed/60;
            foreach (var attendee in updateTotal.Meeting.Attendees)
            {
                var hourlyRate = GetHourlyRate(attendee);
                total += hourlyRate*(decimal) hourlyPercent;
            }
            updateTotal.Meeting.Total = total;
            var meeting = _meetingRepository.Save(updateTotal.Meeting);

            return await Task.FromResult(meeting);
        }

        private decimal GetHourlyRate(User user)
        {
            if (user.Salary > 0)
            {
                return user.Salary/2080;
            }
            return user.HourlyRate;
        }
    }
}
