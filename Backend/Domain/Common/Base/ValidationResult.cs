namespace Domain.Common.Base;

public class ValidationResult
{
    public bool IsSuccessful { get; private set; } = true;
    public List<string> ErrorMessages { get; init; } = new List<string>();
    
    public ValidationResult AddErrors(List<string> errorMessages)
    {
        if (errorMessages.Any())
        {
            IsSuccessful = false; 
        }
        
        ErrorMessages.AddRange(errorMessages);
        return this;
    }
}