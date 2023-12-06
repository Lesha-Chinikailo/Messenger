using serverSignalRChat.hub;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
var app = builder.Build();
app.Urls.Clear();
app.Urls.Add("http://192.168.43.245:5153");


app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<ChatHub>("/chat");

app.Run();

