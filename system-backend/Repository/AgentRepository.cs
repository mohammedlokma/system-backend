using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using system_backend.Data;
using system_backend.Models;
using system_backend.Models.Dtos;
using system_backend.Repository.Interfaces;
using system_backend.Services;

namespace system_backend.Repository
{
    public class AgentRepository : BaseRepository<Agent>, IAgentRepository
    {

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private IAuthServices _authServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public AgentRepository(ApplicationDbContext db,
            IMapper mapper, IAuthServices authServices, UserManager<ApplicationUser> userManager) : base(db)
        {
            _db = db;
            _mapper = mapper;
            _authServices = authServices;
            _userManager = userManager;
        }
        public async Task<AuthModel> RegisterUserAsync(RegisterAgentModel model)
        {
            //here i will make transaction
            var transaction = _db.Database.BeginTransaction();
            try
            {
                var agent_model = _mapper.Map<ApplicationUser>(model);
                var registerModel = _mapper.Map<RegisterModel>(model);
                var user = await _authServices.RegisterAsync(registerModel);
                if (!user.IsAuthenticated)
                {
                    return user;
                }
                model.Id = user.UserId;

                var agent = _mapper.Map<Agent>(model);
                foreach (var place in agent.ServicePlaces)
                {
                    place.AgentId = user.UserId;
                }
                // throw exectiopn to test roll back transaction
                //throw new Exception();

                await _db.Agents.AddAsync(agent);
                _db.SaveChanges();
                transaction.Commit();

                return new AuthModel
                {
                    UserId = agent.Id,
                    Username = user.Username,
                    ExpiresOn = new DateTime(),
                    IsAuthenticated = true,
                    Role = null,
                    Token = null,

                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return new AuthModel { Message = ex.ToString() };

            }

        }
        public async Task<List<AgentDTO>> GetAgentsAsync()
        {

            var users = _userManager.Users;
            var agents = await (from user in _db.Users
                                join userRole in _db.UserRoles
                                on user.Id equals userRole.UserId
                                join role in _db.Roles
                                on userRole.RoleId equals role.Id
                                where role.Name == "User"
                                join agent in _db.Agents
                                on user.Id equals agent.Id

                                select new AgentDTO
                                {
                                    Id = agent.Id,
                                    UserDisplayName = user.UserDisplayName,
                                    UserName = user.UserName,
                                    ServicePlaces = (from a in _db.AgentServicePlaces
                                                     join s in _db.ServicePlaces on a.ServicePlacesId equals s.Id
                                                     where a.AgentId == user.Id
                                                     select s.Name).ToList()
                                }
                                                         )

                   .ToListAsync();

            var agentsDTO = _mapper.Map<List<AgentDTO>>(agents);
            return agentsDTO;

        }
        public async Task<List<AgentDTO>> GetAgentPlacesAsync(int id)
        {

            var users = _userManager.Users;
            var agents = await (from user in _db.Users
                                join userRole in _db.UserRoles
                                on user.Id equals userRole.UserId
                                join role in _db.Roles
                                on userRole.RoleId equals role.Id
                                where role.Name == "User"
                                join agent in _db.Agents
                                on user.Id equals agent.Id
                                join place in _db.AgentServicePlaces
                                on agent.Id equals place.AgentId
                                where place.ServicePlacesId == id
                                select new AgentDTO
                                {
                                    Id = agent.Id,
                                    UserDisplayName = user.UserDisplayName,
                                    UserName = user.UserName,
                                    ServicePlaces = (from a in _db.AgentServicePlaces
                                                     join s in _db.ServicePlaces on a.ServicePlacesId equals s.Id
                                                     where a.AgentId == user.Id
                                                     select s.Name).ToList()
                                }
                                                         )

                   .ToListAsync();

            var agentsDTO = _mapper.Map<List<AgentDTO>>(agents);
            return agentsDTO;

        }
        public async Task<AgentModel> GetAgentAsync(string id)
        {


            var agentModel = await (from user in _db.Users
                                      where user.Id == id
                                      join agent in _db.Agents
                                       on id equals agent.Id
                                      select new AgentModel
                                      {

                                          Id = user.Id,
                                          UserName = user.UserName,
                                          UserDisplayName = user.UserDisplayName,
                                          custody = agent.custody,
                                          Coupons =
                                          _mapper.Map<List<CouponDTO>>
                                          (_db.Coupons.Where(i => i.AgentId == id)
                                          .OrderByDescending(i => i.Id).Take(10).ToList()),

                                          Expenses = _mapper.Map<List<ExpenseDTO>>
                                          (_db.Expenses.Where(i => i.AgentId == id)
                                          .OrderByDescending(i=>i.Id).Take(10).ToList()),
                                          ServicePlaces = (from a in _db.AgentServicePlaces
                                                           join s in _db.ServicePlaces on a.ServicePlacesId equals s.Id
                                                           where a.AgentId == user.Id
                                                           select s.Name).ToList(), 
                                      }).FirstOrDefaultAsync();

            var agentDTO = _mapper.Map<AgentModel>(agentModel);
            return agentDTO;

        }
        public async Task<AgentUpdateDTO> UpdateAsync(AgentUpdateDTO agentDTO)
        {
            var transaction = _db.Database.BeginTransaction();
            try
            {
                var agent = await _userManager.FindByIdAsync(agentDTO.Id);
                agent.UserName = agentDTO.UserName;
                agent.UserDisplayName = agentDTO.UserDisplayName;
                await _userManager.UpdateAsync(agent);
                var places = _db.AgentServicePlaces.Where(i => i.AgentId == agentDTO.Id).ToList();
                _db.AgentServicePlaces.RemoveRange(places);
                foreach (var item in agentDTO.ServicePlaces)
                {
                    var place = new AgentServicePlaces()
                    {
                        AgentId = agentDTO.Id,
                        ServicePlacesId = item.ServicePlacesId
                    };
                    await _db.AgentServicePlaces.AddAsync(place);
                }

                await _db.SaveChangesAsync();
                transaction.Commit();
                return agentDTO;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return agentDTO;
            }
        }

        public async Task DeleteAgentAsync(string id)
        {
            var transaction = _db.Database.BeginTransaction();
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);
                var agent = await _db.Agents.FindAsync(id);
                 _db.Agents.Remove(agent);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }
    }
}
