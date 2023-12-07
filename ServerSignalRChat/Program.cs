using serverSignalRChat.hub;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
var app = builder.Build();
var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
app.Urls.Clear();
app.Urls.Add($"http://{MyConfig.GetValue<string>("serverIp")}:{MyConfig.GetValue<string>("serverPort")}");

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<ChatHub>("/chat");

app.Run();

