using Microsoft.EntityFrameworkCore;
using Project.DataLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProjectDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:SampleJWT").Value);
}, ServiceLifetime.Transient);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
         {
             //zamani ke paramet zir ra false mikonim bedoon https ham mitavanim Authentication anjam dahim
             options.RequireHttpsMetadata = false;
             options.SaveToken = true;// save shodan token

             options.TokenValidationParameters = new TokenValidationParameters
             {
                 //ta 30 sanie bishtar token motabar khahad bood va expire nemishavad
                 //masan agar expire time ma 15 minute bashad token 15:30 etebar khahad dasht
                 ClockSkew = TimeSpan.FromSeconds(30),
                 // mohem ast ke bedanid sader konande token kist?
                 ValidateIssuer = true,
                 //mohem ast ke bedanid daryaft konande token kist?
                 ValidateAudience = true,
                 //expire time validate shavad (expire time dashte bashim ya kheir?)
                 RequireExpirationTime = true,
                 //toole omre token validate shavad?
                 ValidateLifetime = true,
                 //mohem ast ke emzaye token ra validate konam?
                 ValidateIssuerSigningKey = true,
                 //agar sader konande token che kasi bood valid ast?
                 ValidIssuer = "ThisServer",
                 //ValidIssuer = builder.Configuration["Jwt:Issuer"],
                 //agar daryaft konande token che kasi bood valid ast?
                 ValidAudience = "SampleJWTClients_Android",
                 //agar daryaft konande token che kasani boodand valid hastand?
                 //ValidAudiences = new List<string> { "IOS","Android","Windows"},
                 //ValidAudience = builder.Configuration["Jwt:Issuer"],
                 //IssuerSigningKey ra tahvil bede. emzaye sader konande token ra be man bede
                 IssuerSigningKey =
                     // new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                     new SymmetricSecurityKey(Encoding.UTF8.GetBytes("in_key_tavasote_khodam_generate_shode")),
                 TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1111222233334444"))
             };
             options.Events = new JwtBearerEvents {
                 OnTokenValidated = async(context) => {
                     //code khate zir dastresi be database ra faraham mikonad
                     var db = context.HttpContext.RequestServices.GetRequiredService<ProjectDbContext>();  
                     //khate paein list hame claim user ra barmigardanad
                     //ClaimsIdentity ke dar in ja vojod darad haman ClaimsIdentity ke dar class GenerateToken dar subject
                     // descriptor gharar dadim
                     var claims = (context.Principal.Identity as ClaimsIdentity).Claims;
                     var userId = Guid.Parse(claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
                     var userActive = await db.Tbl_Users.Where(u => u.Uid == userId)
                                     .Select(x => x.IsActive).FirstOrDefaultAsync();
                     //tavasote in code barresi kardim ke agar yek user khas active nabood autorize nashavad
                     // va dastresi be method haei ke niaz be autorize daranad nadashte bashad
                     if (!userActive)
                         context.Fail("User Is Not Active");
                 },
             };
         });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
