using SPAWedding.Core.Models;
using SPAWedding.Core.Utility;
using SPAWedding.Infrastructure.Repositories;
using SPAWedding.Infratructure.Services;
using SPAWedding.Web.Providers;
using SPAWedding.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPAWedding.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly InvoicesRepository _invoicesRepository;
        private readonly CustomersRepository _customerRepo;
        private readonly EPaymentRepository _ePaymentRepo;
        private readonly EPaymentLogRepository _ePaymentLogRepo;
        private readonly ProductService _productService;
        private SMSLogRepository _smsLogRepo;

        public PaymentController(InvoicesRepository invoicesRepository, CustomersRepository customersRepository,
            EPaymentRepository ePaymentRepository,
            EPaymentLogRepository ePaymentLogRepository,
            ProductService productService,
            SMSLogRepository smsLogRepository)
        {
            _invoicesRepository = invoicesRepository;
            _customerRepo = customersRepository;
            _ePaymentRepo = ePaymentRepository;
            _ePaymentLogRepo = ePaymentLogRepository;
            _productService = productService;
            _smsLogRepo = smsLogRepository;
        }

        // GET: Payment
        public ActionResult Index()
        {
            return Redirect("/Shop/Checkout");
        }

        [CustomerAuthorize]
        [HttpPost]
        public ActionResult InitPay(string invoiceNumber)
        {
            if(string.IsNullOrEmpty(invoiceNumber))
            {
                return Redirect("/Customer/Dashboard/Invoices");
            }

            Invoice invoice = _invoicesRepository.GetInvoice(invoiceNumber);
            var customer = _customerRepo.GetCurrentCustomer();


            // prevent two payment for a same invoice at the same time
            var prevEPayment = _ePaymentRepo.GetInvoiceLatestUnprocessedEPayment(invoice.Id);
            if(prevEPayment != null)
            {
                if(DateTime.Now.Subtract(prevEPayment.InsertDate.Value).TotalMinutes > 10)
                {
                    // expire the payment and continue
                    prevEPayment.PaymentStatus = PaymentStatus.Failed;
                    var ePayLog = new EPaymentLog();
                    ePayLog.Message = "پایان اعتبار پرداخت پس از سپری شدن بیش از 10 دقیقه";
                    ePayLog.LogDate = DateTime.Now;
                    ePayLog.LogType = "منقضی شد";
                    ePayLog.MethodName = "/Payment/InitPay";
                    ePayLog.Amount = ePayLog.Amount;
                    ePayLog.Token = ePayLog.Token;
                    ePayLog.InsertDate = DateTime.Now;
                    ePayLog.InsertUser = customer.User.UserName;


                    prevEPayment.PaymentStatus = PaymentStatus.Failed;
                    if (prevEPayment.EPaymentLogs == null)
                        prevEPayment.EPaymentLogs = new List<EPaymentLog>();
                    prevEPayment.EPaymentLogs.Add(ePayLog);
                    _ePaymentRepo.Update(prevEPayment);

                }
                else
                {
                    return Redirect("/Payment/Error");
                }
            }


            // adding an ePayment
            EPayment ePayment = new EPayment();
            ePayment.InvoiceId = invoice.Id;
            ePayment.Amount = invoice.TotalPrice;
            ePayment.Description = "پرداخت هزینه سفارش از فروشگاه سه شین گالری";
            ePayment.Token = "";
            ePayment.ExtraInfo = "تکمیل نشده";
            ePayment.PaymentStatus = PaymentStatus.Unprocessed;
            ePayment.InsertUser = customer.User.UserName;
            ePayment.InsertDate = DateTime.Now;
            ePayment.PaymentAccountId = _ePaymentRepo.GetPaymentAccountId();

            _ePaymentRepo.Add(ePayment);


            // logging initial bank response
            IBankGatewayRepository bankGatewayRepository = new PasargadGatewayRepository();
            IDictionary<string, string> data = new Dictionary<string, string>();
            data.Add("InvoiceNumber", ePayment.Id.ToString()); // sending epayment Id as invoice number
            data.Add("InvoiceDate", ePayment.InsertDate.ToString());
            data.Add("Amount", invoice.TotalPrice.ToString());
            data.Add("Mobile", invoice.Phone);
            data.Add("Email", invoice.Email);
            data.Add("Timestamp", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            var result = bankGatewayRepository.SendInitialRequest(data);
            var bankResult = Newtonsoft.Json.JsonConvert.DeserializeObject<BankResult>(result.RequestResponseMessage);

            // adding an epayment log
            var ePaymentLog = new EPaymentLog();
            ePaymentLog.Message = bankResult.Message;
            ePaymentLog.LogDate = DateTime.Now;
            ePaymentLog.LogType = "تایید سفارش و دریافت توکن پرداخت";
            ePaymentLog.MethodName = "/Payment/InitPay";
            ePaymentLog.Amount = ePayment.Amount;
            ePaymentLog.Token = bankResult.Token;
            ePaymentLog.InsertDate = DateTime.Now;
            ePaymentLog.InsertUser = customer.User.UserName;
            ePaymentLog.AdditionalData = "IsSuccess="+bankResult.IsSuccess + "- Message" + bankResult.Message + "- Token" + (bankResult.Token ?? "");

            ePayment.EPaymentLogs = new List<EPaymentLog>();
            ePayment.EPaymentLogs.Add(ePaymentLog);
            _ePaymentRepo.Update(ePayment);


            if (bankResult.IsSuccess)
            {
                ePayment.Token = bankResult.Token;
                _ePaymentRepo.Update(ePayment);

                var url = bankGatewayRepository.GetPaymentURL(bankResult.Token);
                Response.Write(url);
            }
            else
            {
                return Redirect("/Payment/Error");
            }


            return View();

        }

        [CustomerAuthorize]
        public ActionResult Callback()
        {
            var parameters = Request.QueryString;

            if(string.IsNullOrEmpty(parameters["iN"]) && string.IsNullOrEmpty(parameters["tref"]))//TransactionReferenceID
            {
                return Redirect("/Payment/Error");
            }


            // check if transaction succeed
            var customer = _customerRepo.GetCurrentCustomer();
            if (customer == null) // in case admin tries to buy something
                return Redirect("/Customer/Auth/Register");

            IBankGatewayRepository bankGatewayRepository = new PasargadGatewayRepository();
            var result = bankGatewayRepository.CheckPeyment(parameters);
            var bankResult = Newtonsoft.Json.JsonConvert.DeserializeObject<BankResult>(result.RequestResponseMessage);

            var output = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,string>>(result.RequestResponseMessage);

            if(bool.Parse(output["IsSuccess"]))//succeed
            {
                int paymentId = int.Parse(output["InvoiceNumber"]);// this "InvoiceNumber" is actually the payment id
                var invoice = _invoicesRepository.GetInvoiceByPaymentId(paymentId);
                // getting invoice latest payment
                var payment = _ePaymentRepo.Get(paymentId);


                // invoice number and invoice date are already present in "parameters"
                IDictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Amount", invoice.TotalPrice.ToString());
                data.Add("Timestamp", payment.InsertDate.Value.ToString("yyyy/MM/dd HH:mm:ss"));
                data.Add("tref", parameters["tref"]);
                data.Add("iN", parameters["iN"]);
                data.Add("iD", parameters["iD"]);


                result = bankGatewayRepository.VerifyPeyment(data);
                Response.Write(result.RequestResponseMessage);
                var verifyRes = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(result.RequestResponseMessage);

                if(bool.Parse(verifyRes["IsSuccess"]))
                {
                    // log result & update payment
                    var ePaymentLog = new EPaymentLog();
                    ePaymentLog.Message = verifyRes["Message"];
                    ePaymentLog.LogDate = DateTime.Now;
                    ePaymentLog.LogType = "تایید پرداخت موفق";
                    ePaymentLog.MethodName = "/Payment/Confirm";
                    ePaymentLog.Amount = payment.Amount;
                    ePaymentLog.Token = "";
                    ePaymentLog.InsertDate = DateTime.Now;
                    ePaymentLog.InsertUser = customer.User.UserName;
                    ePaymentLog.StackTraceNo = output["TraceNumber"];
                    ePaymentLog.AdditionalData = "ReferenceNumber=" + output["ReferenceNumber"];

                    if (payment.EPaymentLogs == null)
                        payment.EPaymentLogs = new List<EPaymentLog>();
                    payment.EPaymentLogs.Add(ePaymentLog);
                    payment.SystemTraceNo = output["TraceNumber"];
                    payment.PaymentStatus = PaymentStatus.Succeed;
                    payment.RetrievalRefNo = verifyRes["ShaparakRefNumber"];
                    payment.ExtraInfo = verifyRes["MaskedCardNumber"]; // شماره کارت مسک شده کار
                    payment.Description = verifyRes["HashedCardNumber"]; // شماره کارت hash شده کاربری
                    _ePaymentRepo.Update(payment);


                    // update order payed status
                    invoice.IsPayed = true;
                    _invoicesRepository.Update(invoice);



                    // update stock status
                    foreach(var item in invoice.InvoiceItems)
                    {
                        _productService.DecreaseStockProductCount(item.ProductId, item.MainFeatureId, item.Quantity);
                    }



                    var user = _smsLogRepo.GetCurrentUser();
                    var mobile = user.PhoneNumber;

                    SMSLog smsLog = new SMSLog();
                    smsLog.ReceiverMobileNo = mobile;
                    smsLog.MessageBody = "گالری سه شین\n";
                    smsLog.MessageBody += "سفارش شما به شماره: " + invoice.InvoiceNumber;
                    smsLog.MessageBody += "\nبا موفقیت ثبت گردید";
                    smsLog.SendDateTime = DateTime.Now;
                    smsLog.IsFlash = false;
                    smsLog.PatternCode = "";
                    smsLog.LineNumber = ConfigurationManager.AppSettings.Get("LineNumber");



                    SMSDotIrProvider smsDotIrProvider = new SMSDotIrProvider();
                    smsDotIrProvider.SendSMS(ref smsLog);

                    _smsLogRepo.Add(smsLog);


                    return Redirect("/Payment/Succeed/?invoiceNumber=" + invoice.InvoiceNumber);
                }
                else
                {
                    // log result & expire payment
                    var ePaymentLog = new EPaymentLog();
                    ePaymentLog.Message = verifyRes["Message"];
                    ePaymentLog.LogDate = DateTime.Now;
                    ePaymentLog.LogType = "تایید پرداخت شکست خورد";
                    ePaymentLog.MethodName = "/Payment/Confirm";
                    ePaymentLog.Amount = payment.Amount;
                    ePaymentLog.Token = bankResult.Token;
                    ePaymentLog.InsertDate = DateTime.Now;
                    ePaymentLog.InsertUser = customer.User.UserName;
                    ePaymentLog.AdditionalData = "IsSuccess=" + verifyRes["IsSuccess"] + "- Message" + verifyRes["Message"] + " - Token" + (verifyRes["Token"] ?? "");

                    payment.PaymentStatus = PaymentStatus.Failed;
                    payment.EPaymentLogs.Add(ePaymentLog);
                    payment.PaymentStatus = PaymentStatus.Failed;
                    _ePaymentRepo.Update(payment);

                    // redirect
                    return Redirect("/Payment/Error");
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(parameters["iN"]))
                {
                    var paymentId = int.Parse(parameters["iN"]);
                    var payment = _ePaymentRepo.Get(paymentId);
                    payment.PaymentStatus = PaymentStatus.Failed;
                    _ePaymentRepo.Update(payment);
                }

                return Redirect("/Payment/Error");
            }


        }



        public ActionResult Error()
        {
            return View();
        }



        public ActionResult Succeed()
        {
            string invoiceNumber = !string.IsNullOrEmpty(Request["invoiceNumber"])? Request["invoiceNumber"]:"";

            if (string.IsNullOrEmpty(invoiceNumber))
                return Redirect("/");

            var invoice = _invoicesRepository.GetInvoice(invoiceNumber);
            if (invoice == null)
                invoice = new Invoice();

            return View(invoice);
        }

    }
}