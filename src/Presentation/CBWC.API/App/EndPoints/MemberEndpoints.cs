using CBWC.Application.Features.Member.Create.Models;
using CBWC.Application.Features.Member.Delete.Models;
using CBWC.Application.Features.Member.Get.All.Models;
using CBWC.Application.Features.Member.Get.Details.Models;
using CBWC.Application.Features.Member.Update.Models;
using CBWC.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Swashbuckle.AspNetCore.Annotations;

namespace CBWC.API.App.EndPoints;

public static class MemberEndpoints
{
    public static void MapMemberEndpoints(this IEndpointRouteBuilder route)
    {
        var versionSet = route.NewApiVersionSet(name: EndpointGroupNames.Member).Build();
        var routePrefix = EndpointHelper.Prefix + "member";

        var members = route.MapGroup(prefix: routePrefix)
            .AddFluentValidationFilter()
            .WithApiVersionSet(apiVersionSet: versionSet)
            .HasApiVersion(majorVersion: 1)
            .WithGroupName(EndpointGroupNames.Member)
            .WithOpenApi();

        members.MapGet("/", GetAllMembers)
            .ProducesValidationProblem();

        members.MapPost("/", CreateMember)
            .ProducesProblem(statusCode: StatusCodes.Status409Conflict)
            .ProducesValidationProblem();

        members.MapDelete("/{id}", DeleteMember)
            .ProducesProblem(statusCode: StatusCodes.Status409Conflict)
            .ProducesValidationProblem();

        members.MapPut("/{id}", UpdateMember)
            .ProducesProblem(statusCode: StatusCodes.Status409Conflict)
            .ProducesValidationProblem();

        members.MapGet("/{id}", GetMember)
            .ProducesValidationProblem();

    }

    [SwaggerOperation(
        OperationId = "GetAllMembers",
        Summary = "Get all members",
        Description = "Fetches a list of members filtered by FirstName, LastName, MembershipNumber and CNIC.")]
    private static async Task<Results<Ok<GetAllMemberResponse>, ProblemHttpResult, ValidationProblem, NotFound>> GetAllMembers(
        [AsParameters] GetAllMembersRequest memberFilter,
        ISender sender,
        IValidator<GetAllMembersQuery> validator)
    {
        var query = new GetAllMembersQuery
        {
            FirstName = memberFilter.FirstName,
            LastName = memberFilter.LastName,
            MembershipNumber = memberFilter.MembershipNumber,
            CNIC = memberFilter.CNIC,
            PageNumber = memberFilter.PageNumber ?? 1,
            PageSize = memberFilter.PageSize ?? 10
        };

        var validationResult = await validator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await sender.Send(query);

        return TypedResults.Ok(result.Payload);
    }

    [SwaggerOperation(
        OperationId = "CreateMember",
        Summary = "Create a new member",
        Description = "Creates a new member.")]
    private static async Task<Results<Ok<CreateMemberResponse>, ProblemHttpResult, ValidationProblem>> CreateMember(
        CreateMemberRequest model,
        ISender sender)
    {
        var result = await sender.Send(model);

        if (result.ResultCode > 0)
        {
            return TypedResults.Problem(
                title: result.Message,
                statusCode: StatusCodes.Status409Conflict);
        }

        return TypedResults.Ok(result.Payload);
    }

    [SwaggerOperation(
        OperationId = "DeleteMember",
        Summary = "Delete member")]
    private static async Task<Results<Ok<DeleteMemberResponse>, ProblemHttpResult, ValidationProblem, NotFound>> DeleteMember(
        int id,
        ISender sender,
        IValidator<DeleteMemberRequest> validator)
    {
        var model = new DeleteMemberRequest()
        {
            Id = id
        };

        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await sender.Send(model);

        if (result.ResultCode > 0)
        {
            if (result.ResultCode == ResultCodes.NotFound)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Problem(
                title: result.Message,
                statusCode: StatusCodes.Status409Conflict);
        }

        return TypedResults.Ok(result.Payload);
    }

    [SwaggerOperation(
        OperationId = "UpdateMember",
        Summary = "Update member")]
    private static async Task<Results<Ok<UpdateMemberResponse>, ProblemHttpResult, ValidationProblem, NotFound>> UpdateMember(
        int id,
        UpdateMemberRequest model,
        ISender sender,
        IValidator<UpdateMemberCommand> validator)
    {
        var command = new UpdateMemberCommand()
        {
            Id = id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            MembershipNumber = model.MembershipNumber,
            CNIC = model.CNIC
        };

        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var result = await sender.Send(command);
        if (result.ResultCode > 0)
        {
            if (result.ResultCode == ResultCodes.NotFound)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Problem(
                title: result.Message,
                statusCode: StatusCodes.Status409Conflict);
        }

        return TypedResults.Ok(result.Payload);
    }

    [SwaggerOperation(
        OperationId = "GetMemberById",
        Summary = "Get member details",
        Description = "Fetches member details by Id.")]
    private static async Task<Results<Ok<GetMemberResponse>, ProblemHttpResult, ValidationProblem, NotFound>> GetMember(
        int id,
        ISender sender,
        IValidator<GetMemberRequest> validator)
    {
        var model = new GetMemberRequest()
        {
            Id = id
        };

        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var response = await sender.Send(model);
        if (response == null || response.Payload == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(response.Payload);
    }

}
