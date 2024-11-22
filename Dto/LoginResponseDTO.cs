namespace OnlineBookShop.Dto
{
    public class LoginResponseDTO
    {
        public string? UserName {  get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string? UserCode {  get; set; }
        public string Role {  get; set; }
        public List<string> Privilages { get; set; }


    }
}
