using System.Collections.Generic;
using ContaAPI.Domain.Interfaces;
using ContaAPI.Domain.Models;
using ContaAPI.Domain.Entities;
using Flunt.Validations;
using ContaAPI.Infra.Shared.Contexts;
using ContaAPI.Infra.Shared.Mapper;
using System;
using System.Text.RegularExpressions;

namespace ContaAPI.Service.Services
{
    public class HistoricService : IServiceHistoric
    {
        private readonly IRepositoryHistoric _repositoryHistoric;

        public HistoricService(IRepositoryHistoric repositoryHistoric) =>
            _repositoryHistoric = repositoryHistoric;

        public IEnumerable<HistoricModel> RecoverAll()
        {
            var historic = _repositoryHistoric.GetAll();
            return historic.ConvertToHistoric();
        }

        public IEnumerable<HistoricModel> RecoverByUserId(Guid userId)
        {
            var historic = _repositoryHistoric.GetByUserId(userId);
            return historic.ConvertToHistoric();
        }
    }
}
