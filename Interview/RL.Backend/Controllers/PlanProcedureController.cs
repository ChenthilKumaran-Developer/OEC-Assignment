using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RL.Backend.Commands;
using RL.Backend.Models;
using RL.Data;
using RL.Data.DataModels;

namespace RL.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class PlanProcedureController : ControllerBase
{
    private readonly ILogger<PlanProcedureController> _logger;
    private readonly RLContext _context;
    private readonly IMediator _mediator;
    public PlanProcedureController(ILogger<PlanProcedureController> logger, RLContext context, IMediator mediator)
    {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [EnableQuery]
    public IEnumerable<PlanProcedure> Get()
    {
        return _context.PlanProcedures;
    }

    [HttpPost("AssignUsersToPlanProcedure")]
    public async Task<IActionResult> AssignUsersToPlanProcedure([FromBody] AssignUsersToPlanProcedureCommand command, CancellationToken token)
    {
        var response = await _mediator.Send(command, token);

        return response.ToActionResult();
    }

    #region GetUserAssign [Code Owner : Chenthilkumaran]
    [HttpGet("UserAssign/{procedureId}")]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<UserAssignPlanProcedure>>> GetAssignUsers(int procedureId)
    {
        List<UserAssignPlanProcedure> _lstUserAssignPlanProcedure = new List<UserAssignPlanProcedure>();
        foreach (var user in _context.UserAssignPlanProcedure)
        {
            if (user.ProcedureId == procedureId)
            {
                _lstUserAssignPlanProcedure.Add(user);
            }
        }

        if (_lstUserAssignPlanProcedure.Count == 0)
        {
            return NotFound();
        }
        return Ok(_lstUserAssignPlanProcedure);
    }
    #endregion

    #region UsersAssignToPlanProcedure [Code Owner : Chenthilkumaran]
    [HttpPost("UsersAssignToPlanProcedure")]
    public async Task<IActionResult> UsersAssignToPlanProcedure([FromBody] AssignUsersToPlanProcedureCommand command, CancellationToken token)
    {
        var response = await _mediator.Send(command, token);

        return response.ToActionResult();
    }
    #endregion

    #region DeleteUserFromPlanProcedure [Code Owner : Chenthilkumaran]
    [HttpDelete("DeleteAssignUser")]
    public async Task<IActionResult> DeleteUsersFromPlanProcedure([FromBody] DeleteUsersFromPlanProcedureCommand command, CancellationToken token)
    {
        var response = await _mediator.Send(command, token);

        return response.ToActionResult();
    }
    #endregion
}
