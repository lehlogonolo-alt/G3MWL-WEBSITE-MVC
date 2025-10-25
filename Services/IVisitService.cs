using G3MWL.Models;
using System.Collections.Generic;

namespace G3MWL.Services
{
    public interface IVisitService
    {
        List<MonthlyVisitDto> GetMonthlyVisits(string siteName);
    }
}



