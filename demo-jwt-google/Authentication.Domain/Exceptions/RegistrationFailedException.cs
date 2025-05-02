namespace Authentication.Domain.Exceptions;

public class RegistrationFailedException(IEnumerable<string> errors) 
    : Exception($"Registratiuon failed with following errors : {string.Join(Environment.NewLine, errors)}")
{
    
}