namespace OnlineBookShop.Dto
{
    public class PrivilageRequestDTO
    {
        public string? privilegeName { get; set; }
        public string? role { get; set; }
        public List<string>? privilegeIds { get; set; }
        public string? privilegeId { get; set; }
        public int? IsActive { get; set; }
    }
}

