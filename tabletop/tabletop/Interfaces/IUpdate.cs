using System;
using System.Collections.Generic;
using tabletop.Models;
using tabletop.ViewModels;

namespace tabletop.Interfaces
{
    public interface IUpdate
    {
        ChannelEvent AddOrUpdate(InputChannelEvent updateStatusContent);

        DateTime FirstMentionByUrlSafeName(string urlSafeName);

        IEnumerable<ChannelEvent> GetTimeSpanByName(string name, DateTime startDateTime, DateTime endDateTime);

        ChannelUser GetChannelUserIdByUrlSafeName(string nameUrlSafe, bool internalRequest);
        IEnumerable<ChannelUser> GetAllChannelUsers();

        GetStatus IsFree(string channelUserId);
        EventsOfficeHoursModel EventsDayView(DateTime day, string urlSafeName);
        EventsOfficeHoursModel EventsRecent(string urlSafeName);

        EventsOfficeHoursModel ParseEvents(List<ChannelEvent> channelEvents, DateTime startDateTime, DateTime endDateTime);

    }
}
