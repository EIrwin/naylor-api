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

                    return Response.AsJson(meeting);
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
    }
}
