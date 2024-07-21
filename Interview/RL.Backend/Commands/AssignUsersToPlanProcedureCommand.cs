using MediatR;
using RL.Backend.Models;

namespace RL.Backend.Commands
{
    public class AssignUsersToPlanProcedureCommand : IRequest<ApiResponse<Unit>>
    {
        public int ProcedureId { get; set; }
        public List<UserIds> UserIds { get; set; }
    }
    public class UserIds
    {
        public int UserId { get; set; }
    }
}