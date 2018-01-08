using tabletop.Models;

namespace tabletop.Interfaces
{
    public interface IUpdateStatus
    {
        UpdateStatus Add(UpdateStatus UpdateStatusContent);
        UpdateStatus Get(int id);
        UpdateStatus GetLatestByName(string name);
    }
}
