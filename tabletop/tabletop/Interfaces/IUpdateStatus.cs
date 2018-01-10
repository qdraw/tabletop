﻿using System.Collections.Generic;
using tabletop.Models;

namespace tabletop.Interfaces
{
    public interface IUpdateStatus
    {
        UpdateStatus Add(UpdateStatus UpdateStatusContent);
        UpdateStatus Get(int id);
        UpdateStatus GetLatestByName(string name);
        IEnumerable<UpdateStatus> getAll();
        IEnumerable<UpdateStatus> getAllByName(string name);
        IEnumerable<UpdateStatus> getRecentByName(string name);

    }
}
