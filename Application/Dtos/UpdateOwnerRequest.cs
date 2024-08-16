using Domain.Entities;

namespace Application.Dtos
{
    public class UpdateOwnerRequest
    {
        public string OwnerName { get; set; } = string.Empty;
    }
}
