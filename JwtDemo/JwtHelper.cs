namespace JwtDemo; // 改成你的 namespace
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

public class JwtHelper
{
    private readonly IConfiguration _config;

    public JwtHelper(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(string userName, List<string> roles, int expireMinutes = 30)
    {
        // !!!!!!!!! 有雷請注意 !!!!!!!!!
        // issuer 與 sign key 不要打中文，會出現 Http Header 錯誤

        // 從 appsettings 讀取簽發者 (issuer) 與 簽章密鑰 (sign key)
        var issuer = _config.GetValue<string>("JwtSettings:Issuer"); ; // JWT 的核發者
        var secretKey = _config.GetValue<string>("JwtSettings:SignKey"); // 簽章用的密鑰

        // 設定 JWT 的 claim
        // JWT 有提供很多種內建的 claim，請參考 PPT
        var claims = new List<Claim>();
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userName));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        foreach (string role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        // 產生 ClaimsIdentity (既定用法)
        var userClaimsIdentity = new ClaimsIdentity(claims);
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey) // 必須把密鑰轉成位元組 (byte) 的格式
        );
        var signingCredentials = new SigningCredentials(
            securityKey, 
            SecurityAlgorithms.HmacSha256Signature // 指定雜湊加密演算法
        );

        // Create SecurityTokenDescriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            //Audience = issuer, // 通常不需要設定觀眾
            //NotBefore = DateTime.Now, // 預設是 DateTime.Now
            //IssuedAt = DateTime.Now, // 預設是 DateTime.Now
            Subject = userClaimsIdentity,
            Expires = DateTime.Now.AddMinutes(expireMinutes),
            SigningCredentials = signingCredentials
        };

        // 建立 JwtSecurityTokenHandler 並寫出 JWT 的 Token 字串 (固定用法)
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var serializeToken = tokenHandler.WriteToken(securityToken);

        return serializeToken;
    }
}