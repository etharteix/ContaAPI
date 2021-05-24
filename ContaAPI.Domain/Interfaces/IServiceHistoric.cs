using System;
using System.Collections.Generic;
using ContaAPI.Domain.Models;

namespace ContaAPI.Domain.Interfaces
{
    public interface IServiceHistoric
    {
        IEnumerable<HistoricModel> RecoverByUserId(Guid userId);

        IEnumerable<HistoricModel> RecoverAll();
    }
}
