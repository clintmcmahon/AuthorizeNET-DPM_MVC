using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthorizeNet;

namespace AuthorizeNET_DPM_MVC.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var apiLogin = ConfigurationManager.AppSettings["AuthorizeNet.LoginId"];
            var transactionKey = ConfigurationManager.AppSettings["AuthorizeNet.TransactionKey"];
            var rootUrl = ConfigurationManager.AppSettings["RootUrl"];
            var postBackUrl = ConfigurationManager.AppSettings["AuthorizeNet.PostBackUrl"];
            var test = ConfigurationManager.AppSettings["AuthorizeNet.Test"];
            var supportEmail = ConfigurationManager.AppSettings["SupportEmail"];
            var vm = new AuthorizeNET_DPM_MVC.Models.DPMModel();

            vm.PostBackUrl = postBackUrl;
            vm.Amount = 25;
            vm.Login = apiLogin;
            vm.FPTimeStamp = AuthorizeNet.Crypto.GenerateTimestamp().ToString();
            vm.FPSequence = AuthorizeNet.Crypto.GenerateSequence();
            vm.RelayResponse = "TRUE";
            vm.TestRequest = test;
            if (vm.TestRequest == "TRUE")
            {
                vm.x_card_num = "4111111111111111";
                vm.x_exp_date = DateTime.Now.AddMonths(1).ToString("MM-yy");
                vm.x_card_code = "123";
            }
            vm.FPHash = AuthorizeNet.Crypto.GenerateFingerprint(transactionKey, apiLogin, 25, vm.FPSequence, vm.FPTimeStamp);
            vm.InvoiceNumber = vm.FPTimeStamp;

            //This relay url must be accessable from the outside. If you are on a test server or local development machine that cannot be access from the outside world this will not work
            vm.RelayUrl = rootUrl + "Account/DPMResponse";
            vm.ReceiptLinkUrl = rootUrl + "Account/PurchaseReceipt";

            return View(vm);
        }


        public ActionResult DPMResponse(FormCollection post)
        {
            var returnUrl = ProcessDPMResponse(post);
            return Content(string.Format("<html><head><script type='text/javascript' charset='utf-8'>window.location='{0}';</script><noscript><meta http-equiv='refresh' content='1;url={0}'></noscript></head><body></body></html>", returnUrl));
        }

        public string ProcessDPMResponse(FormCollection post)
        {
            string ApiLogin = ConfigurationManager.AppSettings["AuthorizeNet.LoginId"];
            string TxnKey = ConfigurationManager.AppSettings["AuthorizeNet.TransactionKey"];
            string rootUrl = ConfigurationManager.AppSettings["RootUrl"];
            var cust_id = 0;

            try
            {
                var response = new SIMResponse(post);
                var message = response.Message;
                var invoice = response.InvoiceNumber;

                if (post["x_cust_id"] != null)
                {
                    cust_id = int.Parse(post["x_cust_id"]);
                }

                if (!response.Approved || !response.Validate(ApiLogin, ApiLogin))
                {
                    return rootUrl + "Account/PurchaseFailed?&msg=" + HttpUtility.UrlEncode("Credit card authorization denied: " + message);
                }
                else
                {
                    var invoiceNumber = response.InvoiceNumber;
                    var amountPaid = response.Amount;

                    //Successfull transaction so go do whatever you need to do.                   
                    // The URL to redirect to MUST be absolute
                    return rootUrl + "/PurchaseReceipt&confirmation=" + response.InvoiceNumber;
                }
            }
            catch (Exception ex)
            {
                return rootUrl + "PurchaseFailed?&msg=There was an error in the credit card response.";
            }
        }
    }
}
