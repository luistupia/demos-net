namespace Authentication.Domain.Exceptions;

public class UserAlReadyExistsException(string email) : Exception($"User with email: {email} already exists.")
{
    
}