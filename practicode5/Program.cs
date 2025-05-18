//using Service;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.Configure<GitHubOptions>(
//    builder.Configuration.GetSection("GitHub"));
//builder.Services.AddMemoryCache();
//builder.Services.AddScoped<IGitHubService, GitHubService>();
//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();


using Service;

var builder = WebApplication.CreateBuilder(args);

// ����� ������ ������ �����
builder.Services.AddMemoryCache();

// ����� ������ ���
builder.Services.AddScoped<IGitHubService, GitHubService>();

// ����� ������ MVC
builder.Services.AddControllers();

// ����� Swagger
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

