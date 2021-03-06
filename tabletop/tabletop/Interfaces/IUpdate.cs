﻿using System;
using System.Collections.Generic;
using tabletop.Models;
using tabletop.ViewModels;

namespace tabletop.Interfaces
{
    public interface IUpdate
    {
        //UpdateStatus Add(UpdateStatus updateStatusContent);
        //UpdateStatus Update(UpdateStatus updateStatusContent);
        ChannelEvent AddOrUpdate(InputChannelEvent updateStatusContent);


        DateTime FirstMentionByUrlSafeName(string urlSafeName);
        //IEnumerable<UpdateStatus> GetLastMinute(string name);


        //UpdateStatus Get(int id);
        //UpdateStatus GetLatestByName(string name);
        //IEnumerable<UpdateStatus> GetAll();

        //IEnumerable<UpdateStatus> GetAllByName(string name);
        //IEnumerable<UpdateStatus> GetRecentByName(string name);
        IEnumerable<ChannelEvent> GetTimeSpanByName(string name, DateTime startDateTime, DateTime endDateTime);

        ChannelUser GetChannelUserIdByUrlSafeName(string nameUrlSafe, bool internalRequest);
        IEnumerable<ChannelUser> GetAllChannelUsers();

        GetStatus IsFree(string channelUserId);
        //EventsOfficeHoursModel Events(DateTime startDateTime, DateTime endDateTime, string urlSafeName);
        EventsOfficeHoursModel EventsDayView(DateTime day, string urlSafeName);
        EventsOfficeHoursModel EventsRecent(string urlSafeName);

        EventsOfficeHoursModel ParseEvents(List<ChannelEvent> channelEvents, DateTime startDateTime,
            DateTime endDateTime);



    }
}
