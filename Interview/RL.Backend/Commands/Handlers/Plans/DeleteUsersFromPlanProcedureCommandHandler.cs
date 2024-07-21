using MediatR;
using RL.Backend.Exceptions;
using RL.Backend.Models;
using RL.Data;
using System.Data.Entity;

namespace RL.Backend.Commands.Handlers.Plans
{
    public class DeleteUsersFromPlanProcedureCommandHandler : IRequestHandler<DeleteUsersFromPlanProcedureCommand, ApiResponse<Unit>>
    {
        private readonly RLContext _context;

        public DeleteUsersFromPlanProcedureCommandHandler(RLContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<Unit>> Handle(DeleteUsersFromPlanProcedureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate request
                if (request.UserProcedureId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Not Found"));               

                var assignments = _context.UserAssignPlanProcedure.Where(w => w.UserProcedureId == request.UserProcedureId).ToList();

                if (!assignments.Any())
                    return ApiResponse<Unit>.Fail(new NotFoundException("No assignments found for the given ProcedureId and UserIds"));

                _context.UserAssignPlanProcedure.RemoveRange(assignments);
                await _context.SaveChangesAsync(cancellationToken);

                return ApiResponse<Unit>.Succeed(new Unit());
            }
            catch (Exception e)
            {
                return ApiResponse<Unit>.Fail(e);
            }
        }
    }
}
