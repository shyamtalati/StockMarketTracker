using StockMarketTracker.services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpClient<StockService>(client =>
{
    client.BaseAddress = new Uri("https://api.polygon.io");
});

builder.Services.AddSingleton(provider =>
    new StockService(provider.GetRequiredService<HttpClient>(), builder.Configuration["Polygon:ApiKey"])); // Possible Null

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
