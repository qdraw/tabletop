using System;
using System.Collections.Generic;
using tabletop.Models;

namespace tabletop.Interfaces
{
    public interface IUpdateStatus
    {
        //UpdateStatus Add(UpdateStatus updateStatusContent);
        //UpdateStatus Update(UpdateStatus updateStatusContent);
        UpdateStatus AddOrUpdate(UpdateStatus updateStatusContent);


        //IEnumerable<UpdateStatus> GetLastMinute(string name);


        //UpdateStatus Get(int id);
        //UpdateStatus GetLatestByName(string name);
        //IEnumerable<UpdateStatus> GetAll();
        IEnumerable<string> GetUniqueNames();
        //IEnumerable<UpdateStatus> GetAllByName(string name);
        //IEnumerable<UpdateStatus> GetRecentByName(string name);
        IEnumerable<UpdateStatus> GetTimeSpanByName(string name, DateTime startDateTime, DateTime endDateTime);

        GetStatus IsFree(string name);

        
    }
}
