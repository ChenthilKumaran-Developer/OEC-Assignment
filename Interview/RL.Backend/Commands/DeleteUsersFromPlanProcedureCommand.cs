using MediatR;
using RL.Backend.Models;
namespace RL.Backend.Commands
{
    public class DeleteUsersFromPlanProcedureCommand : IRequest<ApiResponse<Unit>>
    {
        public int UserProcedureId { get; set; }
    }
}
