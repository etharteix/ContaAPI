using ContaAPI.Domain.ValueTypes;
using Flunt.Validations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ContaAPI.Domain.Entities
{
    public class HistoricEntity : BaseEntity<Guid>
    {
        public HistoricEntity(Guid id, DateTime movementDate, string movementType, decimal movementValue, Guid userId) : base(id)
        {
            MovementDate = movementDate;
            MovementType = movementType;
            MovementValue = movementValue;
            UserId = userId;
        }

        protected HistoricEntity() { }

        public string MovementType { get; }

        public DateTime MovementDate { get; }

        public decimal MovementValue { get; }

        public Guid UserId { get; }
    }
}