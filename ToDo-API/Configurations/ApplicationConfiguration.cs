namespace ToDo_API.Configurations;

public static class ApplicationConfiguration
{
    public static void ConfigureApplication(WebApplication app)
    {
        app.UseCors("AllowSpecificOrigins");

        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoApi v1"));
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
    }
}