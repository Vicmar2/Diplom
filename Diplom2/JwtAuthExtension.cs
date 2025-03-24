namespace Diplom2;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class JwtAuthExtension
{
    public static void AddJwtAuthentification(this IServiceCollection services)
    {

        var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET");
        //
        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            jwtKey = "YourSuperSecretKeyhhggcjgcfxxffxyhfgvubgilhiogijfyxgxdgxhxgh"; // Это временное значение для тестирования
        }
        //
        services.AddSingleton<JwtTokenHandler>(s => new JwtTokenHandler(jwtKey));

        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false;
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey))
            };
        });


    }


}
