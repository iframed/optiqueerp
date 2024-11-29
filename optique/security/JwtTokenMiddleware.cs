using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

public class JwtTokenMiddleware
{
    private readonly RequestDelegate _next;

    public JwtTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Récupérer le jeton JWT à partir du cookie
        var token = context.Request.Cookies["AuthToken"];
     
        if (!string.IsNullOrEmpty(token))
        {
            // Ajouter le jeton à l'en-tête Authorization
            context.Request.Headers.Add("Authorization", "Bearer " + token);

            // Log pour vérifier si l'en-tête Authorization est correctement configuré
            Console.WriteLine($"Authorization Header: {context.Request.Headers["Authorization"]}");
        }
        else
        {
            Console.WriteLine("No token found in cookie.");
        }

        await _next(context);
    }
}
