namespace Forum.API.ForumUsers
{
    public interface IForumUserContext
    {
        CurrentUser? GetCurrentUser();
    }
}