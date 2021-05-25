using ContaAPI.WebMVC.Models;
using ContaAPI.WebMVC.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace ContaAPI.WebMVC.Controllers
{
    public class AccountController : Controller
    {
      
        // GET: Account
        public ActionResult Index()
        {
            var userId = System.Web.HttpContext.Current.Session["UserId"];
            if (userId == null)
                return RedirectToAction("Login");
            else
                return RedirectToAction("Welcome");
        }

        public ActionResult Register()
        {
            var userId = System.Web.HttpContext.Current.Session["UserId"];
            if (userId != null)
                return RedirectToAction("Welcome");
            return View();
        }

        //The form's data in Register view is posted to this method. 
        //We have binded the Register View with Register ViewModel, so we can accept object of Register class as parameter.
        //This object contains all the values entered in the form by the user.
        [HttpPost]
        public async Task<ActionResult> SaveRegisterDetails(Register registerDetails)
        {
            //We check if the model state is valid or not. We have used DataAnnotation attributes.
            //If any form value fails the DataAnnotation validation the model state becomes invalid.
            if (ModelState.IsValid)
            {
                string apiUrl = "https://localhost:5001/api/users/";
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    string jsonRequest = JsonConvert.SerializeObject(registerDetails);
                    StringContent httpContent = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseText = await response.Content.ReadAsStringAsync();
                        JObject jsonResponse = JObject.Parse(responseText);
                        var userId = jsonResponse.GetValue("id").ToString();
                        var name = jsonResponse.GetValue("name").ToString();
                        System.Web.HttpContext.Current.Session["UserId"] = userId;
                        System.Web.HttpContext.Current.Session["Name"] = name;
                    }
                    else
                    {
                        var responseText = await response.Content.ReadAsStringAsync();
                        responseText = responseText.Trim(new Char[] { '"', '[', ']' });
                        responseText = responseText.Replace("\",\"", " "); 
                        ModelState.AddModelError("Failure", responseText);
                        return View("Register", registerDetails);
                    }
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View("Register", registerDetails);
            }
            return RedirectToAction("Welcome");
        }


        public ActionResult Login()
        {
            var userId = System.Web.HttpContext.Current.Session["UserId"];
            if (userId != null)
                return RedirectToAction("Welcome");
            return View();
        }

        //The login form is posted to this method.
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            string apiUrl = "https://localhost:5001/api/users/login";

            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);

                    string jsonRequest = JsonConvert.SerializeObject(model);
                    StringContent httpContent = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseText = await response.Content.ReadAsStringAsync();
                        JObject jsonResponse = JObject.Parse(responseText);
                        var userId = jsonResponse.GetValue("id").ToString();
                        var name = jsonResponse.GetValue("name").ToString();
                        System.Web.HttpContext.Current.Session["UserId"] = userId;
                        System.Web.HttpContext.Current.Session["Name"] = name;
                    }
                    else
                    {
                        var responseText = await response.Content.ReadAsStringAsync();
                        responseText = responseText.Trim(new Char[] { '\"', '[', ']' });
                        ModelState.AddModelError("Failure", responseText);
                        return View("Login", model);
                    }
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View(model);
            }
            return RedirectToAction("Welcome");
        }

        public ActionResult Logout()
        {
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Welcome()
        {
            var userId = System.Web.HttpContext.Current.Session["UserId"];
            if (userId == null)
                return RedirectToAction("Index");

            var name = System.Web.HttpContext.Current.Session["Name"].ToString();
            var balance = await CheckBalance(userId.ToString());
            var historic = await GetHistoric(userId.ToString());
            var user = new UserBalanceModel(name, balance, historic);

            if (System.Web.HttpContext.Current.Session["errorMsg"] != null)
            {
                ModelState.AddModelError("Failure", System.Web.HttpContext.Current.Session["errorMsg"].ToString());
                System.Web.HttpContext.Current.Session["errorMsg"] = null;
            }
            return View("Welcome", user);
        }

        //function to check if User is valid or not
        public RegisterUser IsValidUser(LoginViewModel model)
        {
            using (var dataContext = new LoginRegistrationInMVCEntities())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                RegisterUser user = dataContext.RegisterUsers.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();
                //If user is present, then true is returned.
                if (user == null)
                    return null;
                //If user is not present false is returned.
                else
                    return user;
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/Account/ResetPassword/" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            using (var context = new LoginRegistrationInMVCEntities())
            {
                var getUser = (from s in context.RegisterUsers where s.Email == EmailID select s).FirstOrDefault();
                if (getUser != null)
                {
                    getUser.ResetPasswordCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    context.Configuration.ValidateOnSaveEnabled = false;
                    context.SaveChanges();

                    var subject = "Password Reset Request";
                    var body = "Hi " + getUser.FirstName + ", <br/> You recently requested to reset your password for your account. Click the link below to reset it. " +

                         " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
                         "If you did not request a password reset, please ignore this email or reply to let us know.<br/><br/> Thank you";

                    SendEmail(getUser.Email, body, subject);

                    ViewBag.Message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    ViewBag.Message = "User doesn't exists.";
                    return View();
                }
            }

            return View();
        }


        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (var context = new LoginRegistrationInMVCEntities())
            {
                var user = context.RegisterUsers.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (var context = new LoginRegistrationInMVCEntities())
                {
                    var user = context.RegisterUsers.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        //you can encrypt password here, we are not doing it
                        user.Password = model.NewPassword;
                        //make resetpasswordcode empty string now
                        user.ResetPasswordCode = "";
                        //to avoid validation issues, disable it
                        context.Configuration.ValidateOnSaveEnabled = false;
                        context.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }

        public async Task<ActionResult> Deposit(UserBalanceModel userModel)
        {
            var userId = System.Web.HttpContext.Current.Session["UserId"];
            string apiUrl = "https://localhost:5001/api/accounts/deposit/" + userId;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                var model = new OperationModel(userModel.Movement);
                string jsonRequest = JsonConvert.SerializeObject(model);
                StringContent httpContent = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(apiUrl, httpContent);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Welcome");
                else
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    responseText = responseText.Trim(new Char[] { '\"', '[', ']' });
                    System.Web.HttpContext.Current.Session["errorMsg"] = responseText;
                    return RedirectToAction("Welcome");
                }
            }
        }

        public async Task<ActionResult> Withdraw(UserBalanceModel userModel)
        {
            var userId = System.Web.HttpContext.Current.Session["UserId"];
            string apiUrl = "https://localhost:5001/api/accounts/withdraw/" + userId;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                var model = new OperationModel(userModel.Movement);
                string jsonRequest = JsonConvert.SerializeObject(model);
                StringContent httpContent = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(apiUrl, httpContent);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Welcome");
                else
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    responseText = responseText.Trim(new Char[] { '\"', '[', ']' });
                    System.Web.HttpContext.Current.Session["errorMsg"] = responseText;
                    return RedirectToAction("Welcome");
                }
            }
        }

        public async Task<ActionResult> Payment(UserBalanceModel userModel)
        {
            var userId = System.Web.HttpContext.Current.Session["UserId"];
            string apiUrl = "https://localhost:5001/api/accounts/payment/" + userId;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                var model = new PaymentModel(userModel.Movement, userModel.PixCode);
                string jsonRequest = JsonConvert.SerializeObject(model);
                StringContent httpContent = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(apiUrl, httpContent);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Welcome");
                else
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    responseText = responseText.Trim(new Char[] { '\"', '[', ']' });
                    System.Web.HttpContext.Current.Session["errorMsg"] = responseText;
                    return RedirectToAction("Welcome");
                }
            }
        }

        public async Task<ActionResult> Monetize(UserBalanceModel userModel)
        {
            var userId = System.Web.HttpContext.Current.Session["UserId"];
            string apiUrl = "https://localhost:5001/api/accounts/monetize/";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);

                HttpResponseMessage response = await client.PostAsync(apiUrl, null);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Welcome");
                else
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    responseText = responseText.Trim(new Char[] { '\"', '[', ']' });
                    System.Web.HttpContext.Current.Session["errorMsg"] = responseText;
                    return RedirectToAction("Welcome");
                }
            }
        }

        private async Task<string> GetHistoric(string userId)
        {
            string apiUrl = "https://localhost:5001/api/historic/" + userId;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseText);
                    string historic = "";
                    DateTime movementDate;
                    string movementType;
                    string movementValue;
                    List<dynamic> childs = new List<dynamic>();
                    foreach (var child in jsonResponse)
                        childs.Add(child);
                    childs.Reverse();
                    foreach (var child in childs)
                    {
                        movementDate = Convert.ToDateTime(child["movementDate"].ToString()).ToLocalTime();
                        movementType = child["movementType"];
                        movementValue = child["movementValue"].ToString("C", CultureInfo.CurrentCulture);
                        historic = historic + "Data: " + movementDate + "\nMovimento: " + movementType + "\nValor: " + movementValue + "\n\n";
                    }
                    return historic;
                }
                else
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    responseText = responseText.Trim(new Char[] { '\"', '[', ']' });
                    ModelState.AddModelError("Failure", responseText);
                    return null;
                }
            }
        }

        private void SendEmail(string emailAddress, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("youremail@gmail.com", emailAddress))
            {
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("youremail@gmail.com", "YourPassword");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);

            }
        }

        private async Task<string> CheckBalance(string userId)
        {
            string apiUrl = "https://localhost:5001/api/accounts/" + userId;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                var responseTexts = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(responseText);
                    var balance = Convert.ToDecimal(jsonResponse.GetValue("balance"));
                    var formattedBalance = balance.ToString("C", CultureInfo.CurrentCulture);
                    return formattedBalance;
                }
                else
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    responseText = responseText.Trim(new Char[] { '\"', '[', ']' });
                    ModelState.AddModelError("Failure", responseText);
                    return null;
                }
            }
        }
    }
}