namespace OnlineBookShop.Dto
{
    public class CustomerResponseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string MobileNumber { get; set; }
        public int OrderCount { get; set; }
        public bool IsActive { get; set; }
    }
}
