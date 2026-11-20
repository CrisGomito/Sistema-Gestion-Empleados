namespace SistemaGestionEmpleados.Application.Common;

/// <summary>
/// Resultado tipado de casos de uso que evita usar excepciones para control de flujo de negocio.
/// </summary>
public class OperationResult
{
    public bool Succeeded { get; init; }
    public string? ErrorMessage { get; init; }
    public string? ErrorField { get; init; }

    public static OperationResult Success() => new() { Succeeded = true };

    public static OperationResult Failure(string message, string? field = null) =>
        new() { Succeeded = false, ErrorMessage = message, ErrorField = field };
}

public sealed class OperationResult<T> : OperationResult
{
    public T? Data { get; init; }

    public static OperationResult<T> Success(T data) =>
        new() { Succeeded = true, Data = data };

    public new static OperationResult<T> Failure(string message, string? field = null) =>
        new() { Succeeded = false, ErrorMessage = message, ErrorField = field };
}
