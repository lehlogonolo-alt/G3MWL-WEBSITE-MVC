using G3MWL.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace G3MWL.Services
{
    public interface ISideEffectService
    {
        int GetTotalSideEffects(); // Optional for dashboard metrics

        Task<List<SideEffect>> GetAllSideEffectsAsync(string token); // Fetch all side effects securely

        Task<bool> CreateSideEffectAsync(SideEffect sideEffect, string token); // Create new side effect via API

        Task<bool> DeleteSideEffectAsync(string id, string token); // Delete side effect by ID via API
    }
}




