var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var client = new HttpClient();
var response = await client.GetAsync("https://en.wikipedia.org/wiki/Special:Random");

var r = await client.GetAsync("http://elasticsearch:9200/");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
