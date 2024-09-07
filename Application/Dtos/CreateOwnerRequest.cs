namespace Application.Dtos
{
    public class CreateOwnerRequest
    {
        public int OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
    }
}
