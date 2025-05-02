namespace Authentication.Domain.Exceptions;

public class ExternalLoginProviderException(string provider, string message) : Exception($"Provider : {provider} Error: {message}");