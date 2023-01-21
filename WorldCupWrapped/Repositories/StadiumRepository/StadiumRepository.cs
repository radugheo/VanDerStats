﻿using WorldCupWrapped.Data;
using WorldCupWrapped.Models;
using WorldCupWrapped.Repository.GenericRepository;

namespace WorldCupWrapped.Repositories.StadiumRepository
{
    public class StadiumRepository : GenericRepository<Stadium>, IStadiumRepository
    {
        public StadiumRepository(ProjectContext context) : base(context)
        {
        }
        public async Task<List<Stadium>> GetAllStadiums()
        {
            return _table.ToList();
        }
    }
}