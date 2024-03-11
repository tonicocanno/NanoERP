using Microsoft.AspNetCore.Mvc;

namespace NanoERP.API.Configurations
{
    public class CustomApiBehaviorOptions
    {
        public static Action<ApiBehaviorOptions> ConfigureInvalidModelStateResponse(int maxErrors = 5)
        {
            return options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToArray();

                    errors = LimitErrors(errors, maxErrors);

                    return new BadRequestObjectResult(new
                    {
                        Message = "One or more validation errors occurred.",
                        Errors = errors
                    });
                };
            };
        }

        private static string[] LimitErrors(string[] errors, int maxErrors)
        {
            if (errors.Length > maxErrors)
            {
                return [.. errors.Take(maxErrors).ToArray(), $"{errors.Length - maxErrors} more errors..."];
            }
            else
            {
                return errors;
            }
        }
    }
}