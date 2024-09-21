namespace kit_stem_api.Models.Domain
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
