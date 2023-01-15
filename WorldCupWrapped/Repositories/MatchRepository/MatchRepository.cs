﻿using WorldCupWrapped.Data;
using WorldCupWrapped.Models;
using WorldCupWrapped.Repository.GenericRepository;

namespace WorldCupWrapped.Repositories.MatchRepository
{
    public class MatchRepository: GenericRepository<Match>, IMatchRepository
    {
        public MatchRepository(ProjectContext context) : base(context)
        {
        }

        /*
        public async Task<List<Match>> GetMatchesByTeamId(Guid teamId)
        {
            return await _context.Matches
                .Where(m => m.HomeTeam == teamId || m.AwayTeam == teamId)
                .ToListAsync();
           
        }
        */
    }
}
