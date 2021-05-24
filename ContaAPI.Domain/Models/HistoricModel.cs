using System;

namespace ContaAPI.Domain.Models
{
    public class HistoricModel
    {
        public HistoricModel(Guid id, DateTime movementDate, string movementType, decimal movementValue, Guid userId)
        {
            Id = id;
            MovementDate = movementDate;
            MovementType = movementType;
            MovementValue = movementValue;
            UserId = userId;
        }

        public Guid Id { get; set; }

        public string MovementType { get; set; }

        public DateTime MovementDate { get; set; }

        public decimal MovementValue { get; set; }

        public Guid UserId { get; set; }
    }
}

