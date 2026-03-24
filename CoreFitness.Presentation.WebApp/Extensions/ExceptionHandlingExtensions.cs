using CoreFitness.Domain.Abstractions.Loggings;
using CoreFitness.Domain.Exceptions;
using CoreFitness.Domain.Exceptions.Custom;
using CoreFitness.Presentation.WebApp.Contracts;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;



namespace CoreFitness.Presentation.WebApp.Extensions;

// Global exception handler för http-request, översätter till HTTP-responser.
public static class ExceptionHandlingExtensions
{
    // Registrerar global exception handling i ASP.NET pipeline
    public static void UseGlobalExceptionHandling(this WebApplication app)
    {
        // Kontrollera om applikationen körs i Development, för mer detaljerade felmeddelanden i utvecklingsmiljö    
        bool isDevelopment = app.Environment.IsDevelopment();

        // ASP.NET middleware som fångar alla exceptions i pipeline.
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                // ASP.NET sparar exceptionet i HttpContext.Features.
                // IExceptionHandlerFeature gör det möjligt att läsa ut exceptionet.
                IExceptionHandlerFeature? exceptionFeature =
                    context.Features.Get<IExceptionHandlerFeature>();

                Exception? exception = exceptionFeature?.Error;

                IDomainLogger logger = context.RequestServices.GetRequiredService<IDomainLogger>();

                if (exception is not null)
                {
                    logger.Log(exception);
                }

                // Alla svar ska vara JSON
                context.Response.ContentType = "application/json";

                // default om inget annat matchar
                int statusCode = StatusCodes.Status500InternalServerError;

                // Standardmeddelande
                string message = "An unexpected error occurred.";

                // Pattern matching på exception-typer
                switch (exception)
                {
                    case ValidationDomainException validationException:

                        statusCode = StatusCodes.Status400BadRequest;
                        message = validationException.Message;

                        break;

                    case NullDomainException nullException:

                        statusCode = StatusCodes.Status400BadRequest;
                        message = nullException.Message;

                        break;

                    case NotFoundDomainException notFoundException:

                        statusCode = StatusCodes.Status404NotFound;
                        message = notFoundException.Message;

                        break;

                    case ConflictDomainException conflictException:

                        statusCode = StatusCodes.Status409Conflict;
                        message = conflictException.Message;

                        break;

                    // Fallback för alla andra DomainExceptions om fler läggs till i framtiden
                    case DomainExceptionBase domainException:

                        statusCode = StatusCodes.Status400BadRequest;
                        message = domainException.Message;

                        break;

                    // Fel i JSON eller request-format
                    case BadHttpRequestException:

                        statusCode = StatusCodes.Status400BadRequest;
                        message = "Invalid request format.";

                        break;

                    case JsonException:
                        // json data i http request body är inte korrekt formaterad
                        statusCode = StatusCodes.Status400BadRequest;
                        message = "Invalid JSON payload.";

                        break;

                    case DbUpdateException:
                        statusCode = StatusCodes.Status409Conflict;
                        break;

                    // Alla andra okända fel
                    default:

                        statusCode = StatusCodes.Status500InternalServerError;

                        if (isDevelopment && exception is not null)
                        {
                            message = exception.Message;
                        }

                        break;
                }

                // Sätt HTTP statuskod
                context.Response.StatusCode = statusCode;

                ErrorResponse errorResponse = new() { Message = message };

                // Skriver till HTTP response body som JSON
                await context.Response.WriteAsJsonAsync(errorResponse);
            });
        });
    }
}