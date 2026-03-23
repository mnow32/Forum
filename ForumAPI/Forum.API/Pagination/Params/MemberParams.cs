namespace Forum.API.Pagination.Params
{
    public class MemberParams : PagingParams
    {
        public string DisplayName { get; set; } = string.Empty;
        public string CurrentMemberId { get; set; } = string.Empty;
    }
}
