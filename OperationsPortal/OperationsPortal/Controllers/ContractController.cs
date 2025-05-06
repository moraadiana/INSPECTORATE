using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OperationsPortal.Models;

using System.Web.Mvc;
using OperationsPortal.NAVWS;
using System.Diagnostics.Contracts;

namespace OperationsPortal.Controllers
{
    public class ContractController : Controller
    {
        private readonly string[] strLimiters2 = new string[] { "[]" };
        private readonly string[] strLimiters = new string[] { "::" };


        Operations webportals = Components.ObjNav;
        // GET: Contract
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ContractList()
        {
            
            if (Session["customerNo"] == null) return RedirectToAction("index", "login");

            var Contracts = new List<Contracts>();
            try
            {
                string username = Session["customerNo"].ToString();
                string contractList = webportals.GetClientContracts(username);
                if (!string.IsNullOrEmpty(contractList))
                {
                    string[] contractArr = contractList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string contractinfo in contractArr)
                    {
                        string[] response = contractinfo.Split(strLimiters, StringSplitOptions.None);
                        Contracts clientContract = new Contracts()
                        {
                            No = response[0].Trim(),
                            Description = response[1].Trim(),
                            Process = response[2].Trim(),
                            processType = response[3].Trim(),
                            startDate = response[4].Trim(),
                            endDate = response[5].Trim(),
                            Status = response[6].Trim()

                        };
                        Contracts.Add(clientContract);
                    }
                }
                else
                {
                    ViewBag.Error = "No contracts found for this client.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "dashboard");
            }
            return View(Contracts);
        }
        public ActionResult ContractDetails(string contractNo)
        {
            Session["contractNo"] = contractNo;
            if (Session["customerNo"] == null)
                return RedirectToAction("index", "login");
            
            
            var storageList = new List<Storage>();
            var productList = new List<Product>();
            var partyList = new List<Party>();
            try
            {

                //string username = Session["customerNo"].ToString();
                string contractStorage = webportals.GetContractStorage(contractNo);
                string contractProducts = webportals.GetContractProducts(contractNo);
                string contractParties = webportals.GetContractParties(contractNo);
                
                if (!string.IsNullOrEmpty(contractStorage))
                {
                    string[] contractArr = contractStorage.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string contractinfo in contractArr)
                    {
                        string[] response = contractinfo.Split(strLimiters, StringSplitOptions.None);
                        Storage clientContract = new Storage()
                        {
                            Type = response[0].Trim(),
                            Description = response[1].Trim(),
                            
                        };
                        storageList.Add(clientContract);
                    }
                }
                if (!string.IsNullOrEmpty(contractProducts))
                {
                    string[] contractArr = contractProducts.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string contractinfo in contractArr)
                    {
                        string[] response = contractinfo.Split(strLimiters, StringSplitOptions.None);
                        Product clientContract = new Product()
                        {
                            contractNo = response[0].Trim(),
                            Code = response[1].Trim(),
                            Description = response[2].Trim(),

                        };
                        productList.Add(clientContract);
                    }
                }
                if (!string.IsNullOrEmpty(contractParties))
                {
                    string[] contractArr = contractParties.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string contractinfo in contractArr)
                    {
                        string[] response = contractinfo.Split(strLimiters, StringSplitOptions.None);
                        Party clientContract = new  Party()
                        {
                            contractNo = response[0].Trim(),
                            Type = response[1].Trim(),
                            Name = response[2].Trim(),

                        };
                        partyList.Add(clientContract);
                    }
                }
                ViewBag.StorageList = storageList;
                ViewBag.ProductList = productList;
                ViewBag.PartyList = partyList;

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Unable to load contract details: " + ex.Message;
                return RedirectToAction("ContractList");
            }

            return View();
        }
        public ActionResult ProductLedger(string productNo,string contractNo)
        {
            if (Session["customerNo"] == null) return RedirectToAction("index", "login");
            Session["productNo"] = productNo;
            Session["contractNo"] = contractNo;
            var ProductLedger = new List<ProductLedger>();
            try
            {
                string username = Session["customerNo"].ToString();
                
                string productLedgerList = webportals.GetCMAProductLedger(contractNo,productNo);
                if (!string.IsNullOrEmpty(productLedgerList))
                {
                    string[] productLedgerArr = productLedgerList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string productLedger in productLedgerArr)
                    {
                        string[] response = productLedger.Split(strLimiters, StringSplitOptions.None);
                        ProductLedger productledger = new ProductLedger()
                        {
                            Product = response[0].Trim(),
                            facilityName = response[1].Trim(),
                            vesselName = response[2].Trim(),
                            Description = response[3].Trim(),
                            Quantity = response[4].Trim(),
                            unitCost = response[5].Trim(),
                            UOM = response[6].Trim(),
                            totalValue = response[7].Trim(),

                        };
                        ProductLedger.Add(productledger);
                    }
                }
                else
                {
                    //ViewBag.Error = "No contracts found for this client.";
                    TempData["error"] = "No data . ";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("contract", "contractDetails");
            }
            return View(ProductLedger);
        }

        public ActionResult DispatchOrders()
        {

            if (Session["customerNo"] == null) return RedirectToAction("index", "login");

            var ContractOrders = new List<ContractOrders>();
            try
            {
                string username = Session["customerNo"].ToString();
                string contractList = webportals.GetCustomerContractDispatchOrders(username);
                if (!string.IsNullOrEmpty(contractList))
                {
                    string[] contractArr = contractList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string contractinfo in contractArr)
                    {
                        string[] response = contractinfo.Split(strLimiters, StringSplitOptions.None);
                        ContractOrders clientContract = new ContractOrders()
                        {
                            No = response[0].Trim(),
                            Date = response[1].Trim(),
                            Description = response[2].Trim(),
                            contractNo = response[3].Trim(),
                            Status = response[4].Trim(),
                            

                        };
                        ContractOrders.Add(clientContract);
                    }
                }
                else
                {
                    //ViewBag.Error = "No contracts found for this client.";
                    TempData["error"] = "No Dispatch orders found . ";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "dashboard");
            }
            return View(ContractOrders);
        }
        public ActionResult ReceiptOrders()
        {

            if (Session["customerNo"] == null) return RedirectToAction("index", "login");

            var ReceiptOrders = new List<ContractOrders>();
            try
            {
                string username = Session["customerNo"].ToString();
                string contractList = webportals.GetCustomerContractReceiptOrders(username);
                if (!string.IsNullOrEmpty(contractList))
                {
                    string[] contractArr = contractList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string contractinfo in contractArr)
                    {
                        string[] response = contractinfo.Split(strLimiters, StringSplitOptions.None);
                        ContractOrders clientContract = new ContractOrders()
                        {
                            No = response[0].Trim(),
                            Date = response[1].Trim(),
                            Description = response[2].Trim(),
                            contractNo = response[3].Trim(),
                            Status = response[4].Trim(),


                        };
                        ReceiptOrders.Add(clientContract);
                    }
                }
                else
                {
                    //ViewBag.Error = "No contracts found for this client.";
                    TempData["error"] = "No Receipt orders found. ";
                }
            }
            catch (Exception ex)
            {


                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "dashboard");
            }
            return View(ReceiptOrders);
        }
        public ActionResult ReleaseOrders()
        {

            if (Session["customerNo"] == null) return RedirectToAction("index", "login");

            var ReleaseOrders = new List<ContractOrders>();
            try
            {
                string username = Session["customerNo"].ToString();
                string contractList = webportals.GetCustomerContractReleaseOrders(username);
                if (!string.IsNullOrEmpty(contractList))
                {
                    string[] contractArr = contractList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string contractinfo in contractArr)
                    {
                        string[] response = contractinfo.Split(strLimiters, StringSplitOptions.None);
                        ContractOrders clientContract = new ContractOrders()
                        {
                            No = response[0].Trim(),
                            Date = response[1].Trim(),
                            Description = response[2].Trim(),
                            contractNo = response[3].Trim(),
                            Status = response[4].Trim(),


                        };
                        ReleaseOrders.Add(clientContract);
                    }
                }
                else
                {
                    //ViewBag.Error = "No contracts found for this client.";
                    TempData["error"] = "No Receipt orders found. ";
                }
            }
            catch (Exception ex)
            {


                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "dashboard");
            }
            return View(ReleaseOrders);
        }
        public ActionResult TranferOrders()
        {

            if (Session["customerNo"] == null) return RedirectToAction("index", "login");

            var TranferOrders = new List<ContractOrders>();
            try
            {
                string username = Session["customerNo"].ToString();
                string contractList = webportals.GetCustomerContractReleaseOrders(username);
                if (!string.IsNullOrEmpty(contractList))
                {
                    string[] contractArr = contractList.Split(strLimiters2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string contractinfo in contractArr)
                    {
                        string[] response = contractinfo.Split(strLimiters, StringSplitOptions.None);
                        ContractOrders clientContract = new ContractOrders()
                        {
                            No = response[0].Trim(),
                            Date = response[1].Trim(),
                            Description = response[2].Trim(),
                            contractNo = response[3].Trim(),
                            Status = response[4].Trim(),


                        };
                        TranferOrders.Add(clientContract);
                    }
                }
                else
                {
                    //ViewBag.Error = "No contracts found for this client.";
                    TempData["error"] = "No Tranfer orders found. ";
                }
            }
            catch (Exception ex)
            {


                TempData["Error"] = ex.Message;
                return RedirectToAction("index", "dashboard");
            }
            return View(TranferOrders);
        }
    }
}