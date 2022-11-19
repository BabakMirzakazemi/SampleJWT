using Microsoft.IdentityModel.Tokens;
using Project.DataLayer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace SampleJWT.Services
{
    public class GenerateToken
    {
        public static string GetToken(User user)
        {
            //claim be maenaye vizhigi haye karbar. masalan name,Role,Tell,Mobile,Address,Email va.. ke dar
            // khate zir be shekle salighe ei mavarede zir ra entekhab karde eim
            var claims = new List<Claim>(){
                new Claim(ClaimTypes.NameIdentifier,user.Uid.ToString()),
                new Claim(ClaimTypes.Name,user.FullName),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim("MyClaimCustome","HarChiDoostDaram")
            };
            //asle amniat JWT be hamin kelidy ast ke dar khate paein vared mikonim va security key ra misazim
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("in_key_tavasote_khodam_generate_shode"));
            //Credentials be maenaye etebar nameh ast dar khate paein yek sign in motabar ra ba estefade az 
            // security key ke sakhtim va yek alghoritm motabar misazim
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            #region amniat_khandan
            //dar khate paein mikhahim ba encrypt ke eijad mikonim kary konim amniat khandan token ra ham eijad konim ta
            // dar site https://jwt.io/#debugger-io in token ghabel khandan nabashad
            //in kelid bayad daghighan 16 char bashad
            var encriptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1111222233334444"));
            var encriptionCredential = new EncryptingCredentials(encriptionKey, 
                SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);
            #endregion amniat_khandan
            //khate paein mavaredi ast ke gharar ast dar token jwt zakhire konim
            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),// ba in khat hame ie claim ha ra tahvil dadim
                Audience = "SampleJWTClients_Android",// user haye in token che kasani hastand? masalan IOS,Android va...
                Issuer = "ThisServer",// sader konande in token kist?
                IssuedAt = DateTime.Now,// tarikh o zaman sader shodan token
                Expires = DateTime.Now.AddDays(15),// zaman enghezaye token?
                //NotBefore = DateTime.Now.AddDays(1),//dastan simkart, in token baed az 1 rooz faaal shavad
                SigningCredentials = signingCredentials,//etebar nameh ke sakhtim ra inja tahvil midahim
                EncryptingCredentials = encriptionCredential,// ba in khat mohtavaye token ra encryot kardim be vasile
                // encrypty ke dar region amniat_khandan eijad kardim
                CompressionAlgorithm = CompressionAlgorithms.Deflate//chon data ra encrypt mikonim toolani mishavad
                // be vasile in khat kami feshorde mikonim data ra
            };
            //baraye sakhte token be yek JwtSecurityTokenHandler niaz darim ke dar khat zir an ra misazim
            var tokenHandler = new JwtSecurityTokenHandler();
            //dar khate paein token ra misazim
            //CreateToken migooyad etelaat token ra be man bede ma ham migooeim etelat dar descriptor ast
            var securityToken = tokenHandler.CreateToken(descriptor);
            //dar nahayat token ra bayad bargardanim ke be shekle zir an ra barmigardanim
            return tokenHandler.WriteToken(securityToken);
        }
    }
}
