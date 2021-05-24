using System;
using System.Collections.Generic;
using ContaAPI.Domain.Entities;
using ContaAPI.Domain.Interfaces;
using ContaAPI.Infra.Data.Context;
using System.Linq;

namespace ContaAPI.Infra.Data.Repository
{
    public class HistoricRepository : BaseRepository<HistoricEntity, Guid>, IRepositoryHistoric
    {
        public HistoricRepository(MySqlContext mySqlContext) : base(mySqlContext)
        {
        }

        public void Save(HistoricEntity obj) =>
            base.Insert(obj);

        public IList<HistoricEntity> GetAll() =>
            base.Select();

        public IList<HistoricEntity> GetByUserId(Guid userId) =>
            _mySqlContext.Set<HistoricEntity>().Where(movement => movement.UserId == userId).ToList();
    }
}
