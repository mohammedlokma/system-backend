using AutoMapper;
using system_backend.Data;
using system_backend.Models;
using system_backend.Repository.Interfaces;

namespace system_backend.Repository
{
    public class AgentRepository:BaseRepository<Agent>,IAgentRepository
    {
        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;

        public AgentRepository(ApplicationDbContext db, IMapper mapper) : base(db)
        {
            _db = db;
            _mapper = mapper;

        }

    }
}
