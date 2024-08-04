namespace RetailECommercePlatform.Data.Errors;

public class RetailECommerceApiException : Exception
{
    private string ErrorMessage { get; }
    
    public RetailECommerceApiException(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}