using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PMMC.Configs;
using PMMC.Entities;
using PMMC.Helpers;
using PMMC.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PMMC.UnitTests
{
    /// <summary>
    /// Tests for start up
    /// </summary>
    [TestClass]
    public class StartupTests:BaseTest
    {
        /// <summary>
        /// The login url
        /// </summary>
        internal const string LoginUrl = "/api/ver1/auth/login";
        
        /// <summary>
        /// The default view limits url
        /// </summary>
        internal const string DefaultViewLimitsUrl = "/api/ver1/configuration/defaultViewLimits";
        
        /// <summary>
        /// The system values url
        /// </summary>
        internal const string SystemValuesUrl = "/api/ver1/lookup/systemValues";
        
        /// <summary>
        /// The http client for test
        /// </summary>
        readonly HttpClient _client;
        

        /// <summary>
        /// Constructor for start up tests
        /// </summary>
        public StartupTests()
        {
            var testWebHostBuilder = TestHelper.GetWebHostBuilder(TestHelper.GetEnvironment());
            var testServer = new TestServer(testWebHostBuilder);
            _client = testServer.CreateClient();
        }

        /// <summary>
        /// Test login with empty body
        /// </summary>
        [TestMethod]
        public async Task LoginWithEmptyBody()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, LoginUrl);
            request.Content = GetStringContent(new { });
            var result = await _client.SendAsync(request);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var content = await result.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<ApiErrorModel>(content);
            Assert.AreEqual(json.Message, "The DbName field is required.The Server field is required.The Password field is required.The Username field is required.");
        }

        /// <summary>
        /// Test login with too long name
        /// </summary>
        [TestMethod]
        public async Task LoginWithTooLongName()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, LoginUrl);
            request.Content = GetStringContent(new
            {
                Username = new string('*', 300),
                Password = _testSettings.SiteAdmin.UserPassword,
                Server = _testSettings.ServerName,
                _testSettings.DbName
            });  
            var result = await _client.SendAsync(request);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var json = JsonConvert.DeserializeObject<ApiErrorModel>(await result.Content.ReadAsStringAsync());
            Assert.AreEqual(json.Message, "The field Username must be a string with a maximum length of 255.");
        }

        /// <summary>
        /// Test login with invalid server
        /// </summary>
        [TestMethod]
        public async Task LoginWithInvalidServer()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, LoginUrl);
            request.Content = GetStringContent(new
            {
                _testSettings.SiteAdmin.Username,
                Password = _testSettings.SiteAdmin.UserPassword,
                Server = "Invalid",
                _testSettings.DbName
            });
            var result = await _client.SendAsync(request);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var json = JsonConvert.DeserializeObject<ApiErrorModel>(await result.Content.ReadAsStringAsync());
            StringAssert.StartsWith(json.Message, "Error to connect to database");
        }

        /// <summary>
        /// Test default view limits without jwt token
        /// </summary>
        [TestMethod]
        public async Task DefaultViewLimitsWithoutJwtToken()
        {
            var requestWithoutJwtToken = new HttpRequestMessage(HttpMethod.Get, DefaultViewLimitsUrl);
            var resultWithoutJwtToken = await _client.SendAsync(requestWithoutJwtToken);
            Assert.AreEqual(HttpStatusCode.Unauthorized, resultWithoutJwtToken.StatusCode);
        }

        /// <summary>
        /// Test default view limits with invalid role jwt token
        /// </summary>
        [TestMethod]
        public async Task DefaultViewLimitsWithInvalidRoleJwtToken()
        {
            var authRequest = new AuthRequest
            {
                Server = _testSettings.ServerName,
                DbName = _testSettings.DbName
            };
            var token = JwtHelper.GenerateJwtToken(_appSettings.Jwt, _testSettings.NormalUser, authRequest);
            var requestWithInvalidRoleToken = new HttpRequestMessage(HttpMethod.Get, DefaultViewLimitsUrl);
            requestWithInvalidRoleToken.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var resultWithInvalidRoleToken = await _client.SendAsync(requestWithInvalidRoleToken);
            Assert.AreEqual(HttpStatusCode.Forbidden, resultWithInvalidRoleToken.StatusCode);
        }

        /// <summary>
        /// Test default view limits with null role jwt token
        /// </summary>
        [TestMethod]
        public async Task DefaultViewLimitsWithNullRoleJwtToken()
        {
            var authRequest = new AuthRequest
            {
                Server = _testSettings.ServerName,
                DbName = _testSettings.DbName
            };
            var token = JwtHelper.GenerateJwtToken(_appSettings.Jwt, _testSettings.NullRoleUser, authRequest);
            var requestWithNullRoleToken = new HttpRequestMessage(HttpMethod.Get, DefaultViewLimitsUrl);
            requestWithNullRoleToken.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var resultWithNullRoleToken = await _client.SendAsync(requestWithNullRoleToken);
            Assert.AreEqual(HttpStatusCode.Forbidden, resultWithNullRoleToken.StatusCode);
        }

        /// <summary>
        /// Test default view limits with error server jwt token
        /// </summary>
        [TestMethod]
        public async Task DefaultViewLimitsWithErrorServerJwtToken()
        {
            var authRequest = new AuthRequest
            {
                Server = "ErrorServer",
                DbName = _testSettings.DbName
            };
            var token = JwtHelper.GenerateJwtToken(_appSettings.Jwt, _testSettings.SiteAdmin, authRequest);
            var request = new HttpRequestMessage(HttpMethod.Get, DefaultViewLimitsUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await _client.SendAsync(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        /// <summary>
        /// Test default view limits with invalid jwt config token
        /// </summary>
        [TestMethod]
        public async Task DefaultViewLimitsWithInvalidJwtConfigToken()
        {
            var authRequest = new AuthRequest
            {
                Server = _testSettings.ServerName,
                DbName = _testSettings.DbName
            };
            var config = new JwtConfig
            {
                SecurityKey = "ThisWrongJWTSecurityKey",
                 Issuer =_appSettings.Jwt.Issuer,
                Audience = _appSettings.Jwt.Audience,
                ExpirationTimeInMinutes = _appSettings.Jwt.ExpirationTimeInMinutes
            };
            var token = JwtHelper.GenerateJwtToken(config, _testSettings.SiteAdmin, authRequest);
            var request = new HttpRequestMessage(HttpMethod.Get, DefaultViewLimitsUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await _client.SendAsync(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        /// <summary>
        /// Test default view limits with invalid jwt config token
        /// </summary>
        [TestMethod]
        public async Task DefaultViewLimitsWithExpiredJwtToken()
        {
            var authRequest = new AuthRequest
            {
                Server = _testSettings.ServerName,
                DbName = _testSettings.DbName
            };
            var config = new JwtConfig
            {
                SecurityKey = _appSettings.Jwt.SecurityKey,
                Issuer = _appSettings.Jwt.Issuer,
                Audience = _appSettings.Jwt.Audience,
                ExpirationTimeInMinutes = -60
            };
            var token = JwtHelper.GenerateJwtToken(config, _testSettings.SiteAdmin, authRequest);
            var request = new HttpRequestMessage(HttpMethod.Get, DefaultViewLimitsUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await _client.SendAsync(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, result.StatusCode);
            var json = JsonConvert.DeserializeObject<ApiErrorModel>(await result.Content.ReadAsStringAsync());
            StringAssert.StartsWith(json.Message, "Jwt token expired");
        }

        /// <summary>
        /// Test default view limits with invalid jwt token schema
        /// </summary>
        [TestMethod]
        public async Task DefaultViewLimitsWithInvalidJwtTokenSchema()
        {
            var siteAdminToken = await GetSiteAdminToken();
            var request = new HttpRequestMessage(HttpMethod.Get, DefaultViewLimitsUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", siteAdminToken);
            var result = await _client.SendAsync(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        /// <summary>
        /// Test default view limits with valid jwt token
        /// </summary>
        [TestMethod]
        public async Task DefaultViewLimitsWithValidJwtToken()
        {
            var siteAdminToken = await GetSiteAdminToken();
            var requestWithJwtToken = new HttpRequestMessage(HttpMethod.Get, DefaultViewLimitsUrl);
            requestWithJwtToken.Headers.Authorization = new AuthenticationHeaderValue("Bearer", siteAdminToken);
            var resultWithJwtToken = await _client.SendAsync(requestWithJwtToken);
            // now can call api successfully 
            resultWithJwtToken.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<DefaultViewLimits>(await resultWithJwtToken.Content.ReadAsStringAsync());
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test system values with null code type
        /// </summary>
        [TestMethod]
        public async Task SystemValuesWithNullCodeType()
        {
            var siteAdminToken = await GetSiteAdminToken();
            var request = new HttpRequestMessage(HttpMethod.Get, SystemValuesUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", siteAdminToken);
            var result = await _client.SendAsync(request);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var json = JsonConvert.DeserializeObject<ApiErrorModel>(await result.Content.ReadAsStringAsync());
            StringAssert.StartsWith(json.Message, "codeType cannot be null");
        }

        /// <summary>
        /// Test system values with invalid code type
        /// </summary>
        [TestMethod]
        public async Task SystemValuesWithInvalidCodeType()
        {
            var siteAdminToken = await GetSiteAdminToken();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{SystemValuesUrl}?codeType=invalid");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", siteAdminToken);
            var result = await _client.SendAsync(request);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            var json = JsonConvert.DeserializeObject<ApiErrorModel>(await result.Content.ReadAsStringAsync());
            StringAssert.StartsWith(json.Message, "The codeType `invalid` is invalid");
        }


        /// <summary>
        /// Get json string content 
        /// </summary>
        /// <param name="obj">the json object to send</param>
        /// <returns>the json string content</returns>
        private StringContent GetStringContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Get site admin token
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetSiteAdminToken()
        {
            var loginRequest = new HttpRequestMessage(HttpMethod.Post, LoginUrl);
            loginRequest.Content = GetStringContent(new
            {
                Username = _testSettings.SiteAdmin.Username,
                Password = _testSettings.SiteAdmin.UserPassword,
                Server = _testSettings.ServerName,
                DbName = _testSettings.DbName
            });
            var loginResult = await _client.SendAsync(loginRequest);
            loginResult.EnsureSuccessStatusCode();
            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResult.Content.ReadAsStringAsync());
            return authResponse.Token;
        }
    }
}
