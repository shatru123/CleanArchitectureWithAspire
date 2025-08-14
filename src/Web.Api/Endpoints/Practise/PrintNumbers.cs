using System.Globalization;
using Application.Abstractions.Messaging;
using Application.Users.GetById;
using SharedKernel;
using Web.Api.Endpoints.Users;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Practise;

internal sealed class PrintNumbers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("print-numbers/{limit}", async (
            int limit,
            CancellationToken cancellationToken) =>
        {

            if (limit < 1)
            {
                return Result.Failure<string>(new Error("Argument Error", "Limit must be greater than 0.", ErrorType.Validation));
            }
            var numbers = new List<string>();
            for (int i = 1; i <= limit; i++)
            {
                numbers.Add(await GetMultiplicationResult2(i));
            }

            return Result.Success(string.Join(", ", numbers));


        });
    }
    public Task<string> GetMultiplicationResult(int number)
    {
        string result = string.Empty;

        if (number % 3 == 0)
        {
            result = "A";
        }

        if (number % 5 == 0)
        {
            result = "B";
        }

        if (number % 3 == 0 && number % 5 == 0)
        {
            result = "AB";
        }

        if (string.IsNullOrEmpty(result))
        {
            result = number.ToString(CultureInfo.InvariantCulture);
        }

        return Task.FromResult(result);
    }

    private Task<string> GetMultiplicationResult2(int number)
    {
#pragma warning disable IDE0008 // Use explicit type
        var result = (number % 3 == 0 ? "A" : "") + (number % 5 == 0 ? "B" : "");
#pragma warning restore IDE0008 // Use explicit type
        return Task.FromResult(result.Length > 0 ? result : number.ToString(CultureInfo.InvariantCulture));
    }
}
