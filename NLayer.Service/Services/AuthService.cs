using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services;

public class AuthService : IAuthService
{
	private readonly UserManager<IdentityUser> _userManager;
	private readonly IConfiguration config;

	public AuthService(UserManager<IdentityUser> userManager,IConfiguration config)
	{
		_userManager = userManager;
		this.config = config;
	}



	public async Task<bool> RegisterUser(LoginUser loginUser)
	{
		var identityUser = new IdentityUser
		{
			UserName = loginUser.UserName,
			Email = loginUser.UserName
		};

		var result = await _userManager.CreateAsync(identityUser, loginUser.Password);
		return result.Succeeded;
	}


	public async Task<bool> Login(LoginUser loginUser)
	{
		var identityUser =await _userManager.FindByEmailAsync(loginUser.UserName);
		if(identityUser is null)
		{
			return false;
		}

		return await _userManager.CheckPasswordAsync(identityUser, loginUser.Password);
	}

	public string GenerateTokenString(LoginUser loginUser)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Email,loginUser.UserName),
			new Claim(ClaimTypes.Role,"Admin")
		};

		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:Key").Value));

		SigningCredentials signingCred = new SigningCredentials(
			key: securityKey,
			SecurityAlgorithms.HmacSha512Signature
			);


		var securityToken = new JwtSecurityToken(
			claims:claims,
			expires:DateTime.Now.AddMinutes(60),
			issuer: config.GetSection("Jwt:Issuer").Value,
			audience: config.GetSection("Jwt:Audience").Value,
			signingCredentials:signingCred
			
			);


		string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
		return tokenString;
	}
}
