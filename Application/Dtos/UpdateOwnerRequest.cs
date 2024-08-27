namespace Application.Dtos
{
    public class UpdateOwnerRequest
    {
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
    }
}
