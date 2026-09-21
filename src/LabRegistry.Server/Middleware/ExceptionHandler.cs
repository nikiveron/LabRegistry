using LabRegistry.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net;

namespace LabRegistry.Server.Middleware;

public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        List<ExceptionResponse> response = exception switch
        {
            HttpErrorException httpErrorException
                => PrepareHttpErrorException(httpErrorException, httpContext),

            DbUpdateConcurrencyException dbConcurrencyEx
                => PrepareDbConcurrencyException(dbConcurrencyEx, httpContext),

            _ when FindInner<PostgresException>(exception) is { } pgEx
                => PreparePostgresException(pgEx, httpContext),

            _ when FindInner<NpgsqlException>(exception) is { } npgsqlEx
                => PrepareNpgsqlException(npgsqlEx, httpContext),

            OperationCanceledException
                => PrepareCancelledException(httpContext),

            _ => PrepareFormattedError(exception, httpContext, HttpStatusCode.InternalServerError)
        };

        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }

    private static T? FindInner<T>(Exception exception) where T : Exception
    {
        var current = exception;
        while (current is not null)
        {
            if (current is T typed) return typed;
            current = current.InnerException;
        }
        return null;
    }

    private List<ExceptionResponse> PrepareHttpErrorException(
        HttpErrorException exception, HttpContext httpContext)
    {
        var statusCode = (int)exception.HttpStatusCode;
        var errorMessage = $"Код ошибки {statusCode}. Ошибка: {exception.ErrorMessage}";
        logger.LogError("{message}", errorMessage);
        httpContext.Response.StatusCode = statusCode;
        return [new(statusCode.ToString(), exception.ErrorMessage)];
    }

    private List<ExceptionResponse> PrepareDbConcurrencyException(
        DbUpdateConcurrencyException exception, HttpContext httpContext)
    {
        logger.LogWarning(exception, "Конфликт конкурентности при обновлении данных");
        httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
        return [new(
            ((int)HttpStatusCode.Conflict).ToString(),
            "Данные были изменены другим пользователем. Обновите страницу и попробуйте снова"
        )];
    }

    private List<ExceptionResponse> PreparePostgresException(
        PostgresException pgEx, HttpContext httpContext)
    {
        var (statusCode, message) = MapPostgresException(pgEx);

        logger.LogError(pgEx,
            "Ошибка PostgreSQL. SqlState: {SqlState}, Message: {Message}, Detail: {Detail}",
            pgEx.SqlState, pgEx.MessageText, pgEx.Detail);

        httpContext.Response.StatusCode = statusCode;
        return [new(statusCode.ToString(), message)];
    }

    private List<ExceptionResponse> PrepareNpgsqlException(
        NpgsqlException exception, HttpContext httpContext)
    {
        logger.LogError(exception, "Не удалось подключиться к PostgreSQL");
        httpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
        return [new(
            ((int)HttpStatusCode.ServiceUnavailable).ToString(),
            "Сервис базы данных временно недоступен. Попробуйте позже"
        )];
    }

    private List<ExceptionResponse> PrepareCancelledException(HttpContext httpContext)
    {
        logger.LogInformation("Запрос отменён клиентом");
        httpContext.Response.StatusCode = 499;
        return [new("499", "Запрос отменён клиентом")];
    }

    private List<ExceptionResponse> PrepareFormattedError(
        Exception exception, HttpContext httpContext, HttpStatusCode httpStatusCode)
    {
        logger.LogError(exception, "Произошла непредвиденная ошибка при обработке запроса");

        var message = exception.InnerException != null
                      && !string.IsNullOrEmpty(exception.InnerException.Message)
            ? exception.InnerException.Message
            : exception.Message;

        httpContext.Response.StatusCode = (int)httpStatusCode;
        return [new(((int)httpStatusCode).ToString(), message)];
    }

    private static (int StatusCode, string Message) MapPostgresException(PostgresException pgEx)
    {
        return pgEx.SqlState switch
        {
            // 23505 — unique_violation (нарушение уникальности)
            "23505" => (
                (int)HttpStatusCode.Conflict,
                "Запись с такими данными уже существует"
            ),

            // 23503 — foreign_key_violation
            "23503" => (
                (int)HttpStatusCode.BadRequest,
                "Связанная запись не найдена"
            ),

            // 23502 — not_null_violation
            "23502" => (
                (int)HttpStatusCode.BadRequest,
                "Обязательное поле не заполнено"
            ),

            // 23514 — check_violation
            "23514" => (
                (int)HttpStatusCode.BadRequest,
                "Нарушено ограничение целостности данных"
            ),

            // 22001 — string_data_right_truncation
            "22001" => (
                (int)HttpStatusCode.BadRequest,
                "Значение поля превышает допустимую длину"
            ),

            // 22003 — numeric_value_out_of_range
            "22003" => (
                (int)HttpStatusCode.BadRequest,
                "Числовое значение выходит за допустимый диапазон"
            ),

            // 40P01 — deadlock_detected
            "40P01" => (
                (int)HttpStatusCode.Conflict,
                "Обнаружена взаимоблокировка. Попробуйте позже"
            ),

            // 40001 — serialization_failure
            "40001" => (
                (int)HttpStatusCode.Conflict,
                "Конфликт сериализации транзакций. Попробуйте снова"
            ),

            // 57014 — query_canceled
            "57014" => (
                499,
                "Запрос отменён"
            ),

            // 53300 — too_many_connections
            "53300" => (
                (int)HttpStatusCode.ServiceUnavailable,
                "Слишком много подключений к базе данных. Попробуйте позже"
            ),

            _ => (
                (int)HttpStatusCode.InternalServerError,
                "Ошибка при работе с базой данных"
            )
        };
    }
}