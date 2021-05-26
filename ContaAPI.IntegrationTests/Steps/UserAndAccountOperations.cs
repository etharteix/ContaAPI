using ContaAPI.Domain.Models;
using ContaAPI.Infra.Data.Repository;
using ContaAPI.IntegrationTests.Drivers;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace ContaAPI.IntegrationTests.Steps
{
    [Binding, Scope(Tag = "userAndAccountOperations")]
    public class UserAndAccountOperations
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private CreateUserModel _createUserModel;
        private UpdateUserModel _updateUserModel;
        private LoginUserModel _loginUserModel;
        private UserModel _userModel;
        private string _userId;
        private AccountModel _accountModel;
        private UpdateAccountModel _updateAccountModel;
        private PaymentAccountModel _paymentAccountModel;

        public UserAndAccountOperations()
        {
            _server = ServerSetup.Setup();
            _client = _server.CreateClient();
        }

        [Given(@"the user's data:")]
        public void GivenTheUsersData(Table table) =>
            _createUserModel = table.CreateInstance<CreateUserModel>();

        [When(@"the register is done")]
        public async Task WhenTheRegisterIsDone()
        {
            await CheckIfTestUserExist();

            try
            {
                var data = JsonConvert.SerializeObject(_createUserModel);
                var stringContent = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("api/users", stringContent);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);

                var result = await response.Content.ReadAsStringAsync();
                dynamic jsonResult = JsonConvert.DeserializeObject(result);
                _userId = Convert.ToString(jsonResult["id"]);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [When(@"i recover the user by id")]
        public async Task GivenIRecoverTheUserById()
        {
            var response = await _client.GetAsync($"api/users/{_userId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            _userModel = JsonConvert.DeserializeObject<UserModel>(result);
        }

        [Then(@"the user exists, returned, on the database")]
        public void ThenTheIdExistsReturnedOnTheDatabase()
        {
            Assert.Equal(_createUserModel.Name, _userModel.Name);
            Assert.Equal(_createUserModel.Email, _userModel.Email);
        }

        [Given(@"the login data:")]
        public void GivenTheLoginData(Table table) =>
            _loginUserModel = table.CreateInstance<LoginUserModel>();

        [When(@"the login is done")]
        public async Task WhenTheLoginIsDone()
        {
            try
            {
                var data = JsonConvert.SerializeObject(_loginUserModel);
                var stringContent = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("api/users/login", stringContent);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Given(@"the user's update data:")]
        public void GivenTheUsersUpdateData(Table table) =>
            _updateUserModel = table.CreateInstance<UpdateUserModel>();

        [When(@"the update is done")]
        public async Task WhenTheUpdateIsDone()
        {
            try
            {
                var data = JsonConvert.SerializeObject(_updateUserModel);
                var stringContent = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"api/users/{_userId}", stringContent);

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [When(@"i recover the updated user by id")]
        public async Task GivenIRecoverTheUpdatedUserById()
        {
            var response = await _client.GetAsync($"api/users/{_userId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            _userModel = JsonConvert.DeserializeObject<UserModel>(result);
        }

        [Then(@"the updated user exists, returned, on the database")]
        public void ThenTheUpdatedIdExistsReturnedOnTheDatabase()
        {
            Assert.Equal(_updateUserModel.Name, _userModel.Name);
            Assert.Equal(_updateUserModel.Email, _userModel.Email);
        }

        [Given(@"the updated login data:")]
        public void GivenTheUpdatedLoginData(Table table) =>
            _loginUserModel = table.CreateInstance<LoginUserModel>();

        [When(@"the updated login is done")]
        public async Task WhenTheUpdatedLoginIsDone()
        {
            try
            {
                var data = JsonConvert.SerializeObject(_loginUserModel);
                var stringContent = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("api/users/login", stringContent);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Given(@"the deposit data:")]
        public void GivenTheDepositData(Table table) =>
            _updateAccountModel = table.CreateInstance<UpdateAccountModel>();

        [When(@"the deposit is done")]
        public async Task WhenTheDepositIsDone()
        {
            try
            {
                var data = JsonConvert.SerializeObject(_updateAccountModel);
                var stringContent = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"api/accounts/deposit/{_userId}", stringContent);

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [When(@"i recover the account by userId")]
        public async Task GivenIRecoverTheAccountByUserId()
        {
            var response = await _client.GetAsync($"api/accounts/{_userId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            _accountModel = JsonConvert.DeserializeObject<AccountModel>(result);
        }

        [Then(@"the deposit resulting value is returned on the database")]
        public void ThenTheDepositResultingValueIsReturnedOnTheDatabase()
        {
            Assert.Equal(2000000, _accountModel.Balance);
        }

        [Given(@"the withdraw data:")]
        public void GivenTheWithdrawData(Table table) =>
            _updateAccountModel = table.CreateInstance<UpdateAccountModel>();

        [When(@"the withdraw is done")]
        public async Task WhenTheWithdrawIsDone()
        {
            try
            {
                var data = JsonConvert.SerializeObject(_updateAccountModel);
                var stringContent = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"api/accounts/withdraw/{_userId}", stringContent);

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Then(@"the withdraw resulting value is returned on the database")]
        public void ThenTheWithdrawResultingValueIsReturnedOnTheDatabase()
        {
            Assert.Equal(1500000, _accountModel.Balance);
        }

        [Given(@"the payment data:")]
        public void GivenThePaymentData(Table table) =>
            _paymentAccountModel = table.CreateInstance<PaymentAccountModel>();

        [When(@"the payment is done")]
        public async Task WhenThePaymentIsDone()
        {
            try
            {
                var data = JsonConvert.SerializeObject(_paymentAccountModel);
                var stringContent = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync($"api/accounts/payment/{_userId}", stringContent);

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Then(@"the payment resulting value is returned on the database")]
        public void ThenThePaymentResultingValueIsReturnedOnTheDatabase()
        {
            Assert.Equal(1000000, _accountModel.Balance);
        }

        [When(@"the monetize is done")]
        public async Task WhenTheMonetizeIsDone()
        {
            try
            {
                var response = await _client.PostAsync("api/accounts/monetize/", null);

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Then(@"the monetize resulting value is returned on the database")]
        public void ThenTheMonetizeResultingValueIsReturnedOnTheDatabase()
        {
            Assert.Equal(Convert.ToDecimal(1000075.6), _accountModel.Balance);
        }

        private async Task CheckIfTestUserExist()
        {
            var loginData = "{\"email\":\"emailtest2@test.test\", \"password\":\"asd1234\"}";
            var loginStringContent = new StringContent(loginData, Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("api/users/login", loginStringContent);
            if (loginResponse.StatusCode == HttpStatusCode.OK)
            {
                var result = await loginResponse.Content.ReadAsStringAsync();
                dynamic jsonResult = JsonConvert.DeserializeObject(result);
                _userId = Convert.ToString(jsonResult["id"]);
                await _client.DeleteAsync($"api/users/{_userId}");
            }
        }

    }
}
