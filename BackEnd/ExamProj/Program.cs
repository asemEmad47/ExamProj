using ExamProj.Auth;
using ExamProj.Services.ExamServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Context;
using System.Text;
using ExamProj.Interfaces.AuthInterfaces;
using ExamProj.Repos.AuthRepos;
using WebApplication1.Interfaces;
using ExamProj.Repos.GeneralRepo;
using ExamProj.Repos.ExamRepo;
using ExamProj.Interfaces.ExamInterfaces;
using ExamProj.Services.AuthServices;
using ExamProj.Services;
using ExamProj.DTOS;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});
var TokenConfig = builder.Configuration.GetSection("TokenConfig").Get<TokenConfig>();


builder.Services.AddDbContext<Context>((options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = TokenConfig.Issuer,
            ValidateAudience = true,
            ValidAudience = TokenConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenConfig.Key)),
        };
    });


builder.Services.AddControllers();


builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddSingleton<FieldsValidator>();
builder.Services.AddSingleton<TokenCreator>();
builder.Services.AddSingleton(TokenConfig);
builder.Services.AddScoped<IAuthorizeRepo, AuthRepo>();
builder.Services.AddScoped<IExamRepo, ExamRepo>();
builder.Services.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepo<>));
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<Context>();

    try
    {
        await context.Database.MigrateAsync();

        var superAdmin = await context.users.FirstOrDefaultAsync(u => u.Email == "asememad984@gmail.com");

        if (superAdmin == null)
        {
            superAdmin = DefaultAdminCreator.GetSuperAdminUser();
            await context.users.AddAsync(superAdmin);
            await context.SaveChangesAsync();
        }
    }
    catch (Exception)
    {

    }

}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
