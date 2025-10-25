using G3MWL.Models;
using System.Collections.Generic;

namespace G3MWL.Services
{
    public interface IAuditService
    {
        int GetLoginCount(string email);
        string GetLastActivity(string email);
        List<UserActivity> GetRecentActivities(int count);
    }
}


