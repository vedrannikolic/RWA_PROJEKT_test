using System;
using Integration_modul.Models;

namespace Integration_modul.Services
{
	public interface IUserRepository
	{
        User Add(UserRegisterRequest request);
        void ValidateEmail(ValidateEmailRequest request);
        Tokens JwtTokens(JwtTokensRequest request);
        
    }
}

