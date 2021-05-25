using System;


namespace ContaAPI.WebMVC.Models
{
    public class UserBalanceModel
    {
        public UserBalanceModel()
        {
        }

        public UserBalanceModel(string name, string balance, string historic = null, decimal ? movement = null, string pixCode = null)
        {
            Name = name;
            Balance = balance;
            Movement = movement;
            PixCode = pixCode;
            Historic = historic;
        }

        public string Name { get; set; }
        public string Balance { get; set; }
        public decimal? Movement { get; set; }
        public string PixCode { get; set; }
        public string Historic { get; set; }
    }
}