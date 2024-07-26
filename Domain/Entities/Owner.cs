namespace Domain.Entities
{
    public class Owner
    {
         public int OwnerId { get; set; }
         public string OwnerName { get; set; } = string.Empty;
         List<Hotel>? Hotels { get; set; }

    }
}
