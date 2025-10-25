using G3MWL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G3MWL.Services
{
    public interface IPatientService
    {
        Task<List<Patient>> GetAllPatientsAsync(string token);
        Task<Patient?> GetPatientByIdAsync(string id, string token);
        Task<bool> CreatePatientAsync(Patient patient, string token);
        Task<bool> AddReportAsync(string id, WeeklyReport report, string token);
        Task<bool> DeletePatientAsync(string id, string token); 
        int GetTotalPatients();
    }
}




