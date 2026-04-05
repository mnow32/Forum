namespace Forum.API.Exceptions.Models
{
    public class ForbiddenException(string message) : Exception(message)
    {
    }
}
