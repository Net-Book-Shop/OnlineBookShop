﻿namespace OnlineBookShop.Dto
{
    public class LoginResponseDTO
    {
        public string? UserName {  get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }


    }
}
