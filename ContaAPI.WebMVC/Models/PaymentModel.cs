using System;


namespace ContaAPI.WebMVC.Models
{
    public class PaymentModel : OperationModel
    {
        public PaymentModel() : base()
        {
        }

        public PaymentModel(decimal? value, string pixCode)
        {
            Value = value;
            PixCode = pixCode;
        }

        public string PixCode { get; set; }
    }
}