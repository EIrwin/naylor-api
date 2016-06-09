using System;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Naylor.Gateway.Models;
using Naylor.Services;

namespace Naylor.Gateway.Modules
{
    public class MeetingModule:NancyModule
    {
        public MeetingModule(IMeetingService meetingService):base("/meetings")
        {
            Get["/{id}", true] = async (ctx, cancel) =>
                {
                    var request = this.Bind<GetMeetingByIdRequest>();

                    var meeting = await meetingService.GetMeetingById(new GetMeetingById()
                        {
                            Id = request.Id
                        });

                    var utcNow = DateTime.UtcNow;
                    var response = new
                        {
                            id = meeting.Id,
                            startUtc = meeting.StartUtc,
                            endUtc = meeting.EndUtc,
                            total = meeting.Total,
                            active = meeting.Active,
                            attendees = meeting.Attendees.Select(p => p.Id),
                            elapsedSeconds = GetElapsedSeconds(meeting.StartUtc,utcNow),
                            elapsedMinutes = GetElapsedMinutes(meeting.StartUtc,utcNow),
                            elapsedHours = GetElapsedHours(meeting.StartUtc,utcNow)
                        };

                    return Response.AsJson(response);
                };

            Post["/", true] = async (ctx, cancel) =>
                {
                    var meeting = await meetingService.CreateMeeting(new CreateMeeting()
                        {

                        });

                    return Response.AsJson(meeting);
                };

            Put["/_join", true] = async (ctx, cancel) =>
                {
                    var request = this.Bind<JoinMeetingRequest>();

                    var meeting = await meetingService.JoinMeeting(new JoinMeeting()
                        {
                            UserId = request.UserId,
                            MeetingId = request.MeetingId
                        });

                    return Response.AsJson(meeting);
                };

            Put["/_leave", true] = async (ctx, cancel) =>
                {
                    var request = this.Bind<LeaveMeetingRequest>();

                    var meeting = await meetingService.LeaveMeeting(new LeaveMeeting()
                        {
                            UserId = request.UserId,
                            MeetingId = request.MeetingId
                        });

                    return Response.AsJson(meeting);
                };

            Put["/_start", true] = async (ctx, cancel) =>
                {
                    var request = this.Bind<StartMeetingRequest>();

                    var meeting = await meetingService.StartMeeting(new StartMeeting()
                        {
                            MeetingId = request.MeetingId
                        });

                    return Response.AsJson(meeting);
                };

            Put["/_end", true] = async (ctx, cancel) =>
                {
                    var request = this.Bind<EndMeetingRequest>();

                    var meeting = await meetingService.EndMeeting(new EndMeeting()
                        {
                            MeetingId = request.MeetingId
                        });

                    return Response.AsJson(meeting);
                };

        }

        private double GetElapsedMinutes(DateTime startUtc,DateTime nowUtc)
        {
            TimeSpan diff = nowUtc - startUtc;
            return diff.TotalMinutes;
        }

        private double GetElapsedHours(DateTime startUtc,DateTime nowUtc)
        {
            TimeSpan diff = nowUtc - startUtc;
            return diff.TotalHours;
        }

        private double GetElapsedSeconds(DateTime startUtc,DateTime nowUtc)
        {
            TimeSpan diff = nowUtc - startUtc;
            return diff.TotalSeconds;
        }
    }
}
