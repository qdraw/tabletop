using System;
using System.Collections.Generic;
using tabletop.Models;

namespace tabletop.Interfaces
{
    public interface IUpdateStatus
    {
        UpdateStatus Add(UpdateStatus updateStatusContent);
        UpdateStatus Update(UpdateStatus updateStatusContent);
        UpdateStatus AddOrUpdate(UpdateStatus updateStatusContent);


        IEnumerable<UpdateStatus> GetLastMinute(string name);


        UpdateStatus Get(int id);
        UpdateStatus GetLatestByName(string name);
        IEnumerable<UpdateStatus> GetAll();
        IEnumerable<string> GetUniqueNames();
        IEnumerable<UpdateStatus> GetAllByName(string name);
        IEnumerable<UpdateStatus> GetRecentByName(string name);
        IEnumerable<UpdateStatus> ListOfWorkDayByName(string name, DateTime dateTime);

        GetStatus IsFree(string name);

    }
}
