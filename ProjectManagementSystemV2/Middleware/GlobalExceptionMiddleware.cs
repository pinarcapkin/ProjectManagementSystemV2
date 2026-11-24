namespace ProjectManagementSystemV2.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString()); // geliştirici konsolda görsün diye (geçici, ileride gerçek logging sistemine geçilebilir)

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var response = new // kullanıcıya dönecek sabit ve güvenli bir JSON nesnesi oluşturuyoruz, genel mesaj içeriyor
            {
                status = 500,
                title = "Beklenmeyen bir sunucu hatası oluştu.",
                detail = "Lütfen daha sonra tekrar deneyin."
            };

            var json = System.Text.Json.JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json); // JSON metnini cevap olarak gönderiyoruz
        }
    }
}