using MediatR;
using Microsoft.EntityFrameworkCore;
using RL.Backend.Exceptions;
using RL.Backend.Models;
using RL.Data;
using RL.Data.DataModels;

namespace RL.Backend.Commands.Handlers.Plans;

public class AssignUsersToPlanProcedureCommandHandler : IRequestHandler<AssignUsersToPlanProcedureCommand, ApiResponse<Unit>>
{
    private readonly RLContext _context;

    public AssignUsersToPlanProcedureCommandHandler(RLContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Unit>> Handle(AssignUsersToPlanProcedureCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate request
            if (request.ProcedureId < 1)
                return ApiResponse<Unit>.Fail(new BadRequestException("Invalid ProcedureId"));
            if (request.UserIds.Count <= 0)
                return ApiResponse<Unit>.Fail(new BadRequestException("User Not Found"));

            var procedure = await _context.Procedures.FirstOrDefaultAsync(p => p.ProcedureId == request.ProcedureId);

            if (procedure is null)
                return ApiResponse<Unit>.Fail(new NotFoundException($"ProcedureId: {request.ProcedureId} not found"));


            foreach(var userId in request.UserIds)
            {
                bool isExists = await _context.UserAssignPlanProcedure
                                .AnyAsync(w => w.ProcedureId == request.ProcedureId && w.UserId == userId.UserId);
                if (!isExists)
                {
                    string userName = _context.Users.Where(w => w.UserId == userId.UserId).Select(s => s.Name).FirstOrDefault();
                    int maxId = _context.UserAssignPlanProcedure.Max(u => (int?)u.UserProcedureId) ?? 0;

                    var userAssignPlanProcedure = new UserAssignPlanProcedure
                    {
                        UserProcedureId = maxId + 1,
                        ProcedureId = request.ProcedureId,
                        UserId = userId.UserId,
                        UserName = userName,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                    };
                    _context.UserAssignPlanProcedure.Add(userAssignPlanProcedure);
                }
                else
                {
                    return ApiResponse<Unit>.Succeed(new Unit());
                }
            }
            await _context.SaveChangesAsync();
                   
            return ApiResponse<Unit>.Succeed(new Unit());
        }
        catch (Exception e)
        {
            return ApiResponse<Unit>.Fail(e);
        }
    }

    
}