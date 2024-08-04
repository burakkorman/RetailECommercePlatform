namespace RetailECommercePlatform.Data.ResponseModels.Command.Auth;

public class LoginCommandResponse
{
    public bool AuthenticateResult { get; set; }
    public string AuthToken { get; set; }
    public DateTime AccessTokenExpireDate { get; set; }
}