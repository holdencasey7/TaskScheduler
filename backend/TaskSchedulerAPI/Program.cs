namespace TaskSchedulerAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        
        if (builder.Environment.IsDevelopment())  
        {  
            Console.WriteLine("Development");
            builder.Services.AddCors(options =>  
            {
                options.AddDefaultPolicy(  
                    policy =>  
                    {  
                        policy.AllowAnyOrigin()  
                            .AllowAnyHeader()  
                            .AllowAnyMethod();  
                    });  
            });  
        }  

        var app = builder.Build();
        
        app.UseCors();
        
        app.Use(async (context, next) =>
        {
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.StatusCode = 204; // No Content
                return;
            }
            await next();
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}