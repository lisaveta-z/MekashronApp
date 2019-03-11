using MekashronApp.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MekashronApp.IICUTech;
using Newtonsoft.Json.Linq;

namespace MekashronApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            //RegisterNewCustomer();

            string goodMessage = "<div class=\"alert alert-dismissible alert-success\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\">&times;</button><strong>Well done!</strong> You successfully read <a href=\"#\" class=\"alert-link\">this important alert message</a>.</div>";
            string errMessage = "<div class=\"alert alert-dismissible alert-danger\"><button type = \"button\" class=\"close\" data-dismiss=\"alert\">&times;</button><strong>Oh snap!</strong> <a href = \"#\" class=\"alert-link\">Change a few things up</a> and try submitting again.</div>";

            bool isSuccess = false;
            string message = ErrorMessage();

            try
            {
                ICUTechClient client = new ICUTechClient();
                var resultJson = client.Login(model.Username, model.Password, "");

                JObject result = JObject.Parse(resultJson);
                IDictionary<string, JToken> res = result;
                if (res.ContainsKey("EntityId"))
                {
                    isSuccess = true;
                    message = GoodMessage(res);
                }
                else
                {
                    message = WrongPasswordMessage();
                }
            }
            catch
            {
                isSuccess = false;
            }

            Response.Write(GetMessage(isSuccess, message));
            return View("Index");
        }

        private void RegisterNewCustomer()
        {
            ICUTechClient client = new ICUTechClient();
            var res = client.RegisterNewCustomer("testAccount@gmail.com", "1234", "TestFirstName", "TestLastName", "+1234567890", 1, 1, "");
        }

        private string GetMessage(bool isSuccess, string message)
        {
            string alertClass = isSuccess ? "success" : "danger";
            return "<div class=\"alert alert-dismissible alert-" + alertClass + "\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\">&times;</button>" + message + "</div>";

        }

        private string ErrorMessage()
        {
            return "<strong>Something went wrong!</strong> An error occurred.";
        }

        private string WrongPasswordMessage()
        {
            return "<strong>You entered incorrect username or password!</strong> Try submitting again.";
        }

        private string GoodMessage(IDictionary<string, JToken> res)
        {
            var message = "<strong>Well done!</strong> Your account information:<ul>";
            message += MessageForProperty("FirstName", res);
            message += MessageForProperty("LastName", res);
            message += MessageForProperty("Mobile", res);
            message += MessageForProperty("Company", res);
            message += MessageForProperty("Address", res);
            message += "</ul>";
            return message;
        }

        private string MessageForProperty(string property, IDictionary<string, JToken> res)
        {
            return res.ContainsKey(property) && !String.IsNullOrEmpty(res[property].ToString()) ? "<li>" + property + ": " + res[property] + "</li>" : "";
        }
    }
}