namespace Application.Dtos
{
    public class UpdateOwnerRequest
    {
        public int OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
    }
}
