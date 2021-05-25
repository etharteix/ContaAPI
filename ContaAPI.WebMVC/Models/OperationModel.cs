using System;


namespace ContaAPI.WebMVC.Models
{
    public class OperationModel
    {
        public OperationModel()
        {
        }

        public OperationModel(decimal? value)
        {
            Value = value;
        }

        public decimal? Value { get; set; }
    }
}