namespace EmployeePortal.ApiHandler
{
    public enum ApiReturnCodes : int
    {
        Success = 200,
        Created = 201,
        SuccessNoBody = 204,
        BadRequest = 400,
        AuthenticationFailed = 401,
        DoesNotExist = 404,
        ValidationFailed = 422
    }
}
