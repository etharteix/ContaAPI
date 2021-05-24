using System;
using System.Collections.Generic;
using ContaAPI.Domain.Entities;

namespace ContaAPI.Domain.Interfaces
{
    public interface IRepositoryHistoric
    {
        void Save(HistoricEntity obj);

        IList<HistoricEntity> GetAll();

        IList<HistoricEntity> GetByUserId(Guid userId);
    }
}

