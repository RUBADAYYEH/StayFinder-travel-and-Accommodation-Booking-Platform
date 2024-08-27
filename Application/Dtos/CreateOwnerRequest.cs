namespace Application.Dtos
{
    public class CreateOwnerRequest
    {
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
    }
}
