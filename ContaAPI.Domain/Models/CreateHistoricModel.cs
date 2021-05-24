using System;

namespace ContaAPI.Domain.Models
{
    public class CreateHistoricModel
    {
        public CreateHistoricModel(string movementType, decimal movementValue, Guid userId)
        {
            MovementDate = DateTime.UtcNow;
            MovementType = movementType;
            MovementValue = movementValue;
            UserId = userId;
        }

        public string MovementType { get; set; }

        public DateTime MovementDate { get; set; }

        public decimal MovementValue { get; set; }

        public Guid UserId { get; set; }
    }
}
