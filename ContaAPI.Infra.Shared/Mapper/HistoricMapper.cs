using System;
using System.Collections.Generic;
using System.Linq;
using ContaAPI.Domain.Entities;
using ContaAPI.Domain.Models;

namespace ContaAPI.Infra.Shared.Mapper
{
    public static class HistoricMapper
    {
        public static HistoricEntity ConvertToHistoricEntity(this CreateHistoricModel historicModel) =>
            new HistoricEntity(Guid.Empty, historicModel.MovementDate, historicModel.MovementType, historicModel.MovementValue, historicModel.UserId);

        public static IEnumerable<HistoricModel> ConvertToHistoric(this IList<HistoricEntity> historic) =>
            new List<HistoricModel>(historic.Select(s => new HistoricModel(s.Id, s.MovementDate, s.MovementType, s.MovementValue, s.UserId)));
    }
}
