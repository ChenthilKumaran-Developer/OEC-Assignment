using FluentAssertions;
using MediatR;
using RL.Backend.Commands;
using RL.Backend.Commands.Handlers.Plans;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using RL.Backend.Exceptions;
using RL.Data;
using RL.Data.DataModels;

namespace RL.Backend.UnitTests
{
    [TestClass]
    public class AssignUsersToPlanProcedureTests
    {
        [TestMethod]
        [DataRow(1, new[] { 1, 2, 3, 4 })]
        public async Task AssignUsersToPlanProcedureTests_DoesntContainsUserProcedure_ReturnsSuccess(int procedureId, int[] userIds)
        {
            // Given
            var userIdsList = userIds.Select(id => new UserIds { UserId = id }).ToList();
            var context = DbContextHelper.CreateContext();
            var sut = new AssignUsersToPlanProcedureCommandHandler(context);
            var request = new AssignUsersToPlanProcedureCommand()
            {
                ProcedureId = procedureId,
                UserIds = userIdsList
            };
            context.Procedures.Add(new Data.DataModels.Procedure
            {
                ProcedureId = procedureId,
                ProcedureTitle = "Test Procedure"
            });
            var usersToAdd = new List<Data.DataModels.User>
            {
                new Data.DataModels.User
                {
                    UserId = 1,
                    Name = "Nick Morrison",
                    CreateDate = new DateTime(1999, 12, 13),
                    UpdateDate = new DateTime(1999, 12, 13)
                },
                new Data.DataModels.User
                {
                    UserId = 2,
                    Name = "Scott Cassidy",
                    CreateDate = new DateTime(1999,12,13),
                    UpdateDate = new DateTime(1999,12,13)
                },
                new Data.DataModels.User
                {
                    UserId = 3,
                    Name = "Tony Bidner",
                    CreateDate = new DateTime(1999,12,13),
                    UpdateDate = new DateTime(1999,12,13)
                },
                new Data.DataModels.User
                {
                    UserId = 4,
                    Name = "Patryk Skwarko",
                    CreateDate = new DateTime(1999,12,13),
                    UpdateDate = new DateTime(1999,12,13)
                },
            };
            context.Users.AddRange(usersToAdd);

            await context.SaveChangesAsync();
            // When
            var result = await sut.Handle(request, new CancellationToken());

            // Then
            foreach (var userId in userIds)
            {
                var userAssign = await context.UserAssignPlanProcedure.FirstOrDefaultAsync(u => u.ProcedureId == procedureId && u.UserId == userId);
                userAssign.Should().NotBeNull();
            }

            result.Value.Should().BeOfType(typeof(Unit));
            result.Succeeded.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(1, new[] { 1, 2, 3, 4 })]
        public async Task AssignUsersToPlanProcedureTests_AlreadyContainsProcedure_ReturnsSuccess(int procedureId, int[] userIds)
        {
            // Given
            var userIdsList = userIds.Select(id => new UserIds { UserId = id }).ToList();
            var context = DbContextHelper.CreateContext();
            var sut = new AssignUsersToPlanProcedureCommandHandler(context);

            var existingProcedure = await context.Procedures.FirstOrDefaultAsync(p => p.ProcedureId == procedureId);
            if (existingProcedure == null)
            {
                context.Procedures.Add(new Data.DataModels.Procedure
                {
                    ProcedureId = procedureId,
                    ProcedureTitle = "Test Procedure"
                });

                context.UserAssignPlanProcedure.Add(new Data.DataModels.UserAssignPlanProcedure
                {
                    UserProcedureId = 1,
                    ProcedureId = 1,
                    UserId = 1,
                    UserName = "Nick Morrison",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                });

                await context.SaveChangesAsync();
            }


            // When
            var request = new AssignUsersToPlanProcedureCommand()
            {
                ProcedureId = procedureId,
                UserIds = userIdsList
            };

            var result = await sut.Handle(request, new CancellationToken());

            // Then           
            result.Value.Should().BeOfType(typeof(Unit));
            result.Succeeded.Should().BeTrue();

        }

        [TestMethod]
        [DataRow(0, 0, new[] { 0 })]
        public async Task AddProcedureToPlanTests_InvalidUserAssignPlanProcedureId_ReturnsBadRequest(int userProcedureId, int procedureId, int[] userIds)
        {
            // Given
            var userIdsList = userIds.Select(id => new UserIds { UserId = id }).ToList();
            var context = DbContextHelper.CreateContext();
            var sut = new AssignUsersToPlanProcedureCommandHandler(context);

            var existingProcedure = await context.Procedures.FirstOrDefaultAsync(p => p.ProcedureId == procedureId);
            if (existingProcedure == null)
            {
                context.Procedures.Add(new Data.DataModels.Procedure
                {
                    ProcedureId = procedureId,
                    ProcedureTitle = "Test Procedure"
                });

                context.UserAssignPlanProcedure.Add(new Data.DataModels.UserAssignPlanProcedure
                {
                    UserProcedureId = userProcedureId,
                    ProcedureId = procedureId,
                    UserId = userIds.Select(s => s).FirstOrDefault(),
                    UserName = "",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                });

                await context.SaveChangesAsync();
            }


            // When
            var request = new AssignUsersToPlanProcedureCommand()
            {
                ProcedureId = procedureId,
                UserIds = userIdsList
            };

            var result = await sut.Handle(request, new CancellationToken());

            // Then           
            result.Value.Should().BeOfType(typeof(Unit));
            result.Succeeded.Should().BeFalse();

        }
    }
}
