using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

using Microsoft.Extensions.Logging;

using MStart_Hiring_Task.Models;

using System.Security.Claims;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using System.IO;

namespace MStart_Hiring_Task.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        /* All Controls that require Admin acess permission */
        [Authorize(Roles = "Admin")]
        public IActionResult Users()
        {
            string allUsers = UsersOperations.getAllUsers();
            var allUsersList = new List<User>();

            try
            {
                string[] rows = allUsers.Split('\n');

                foreach (var row in rows)
                {
                    User elem = new User();
                    string[] columns = row.Split(',');

                    for (int i = 0; i < columns.Length; i++)
                        columns[i] = columns[i].Trim();

                    elem.ID = Int32.Parse(columns[0]);
                    elem.name =           columns[1];
                    elem.email =          columns[2];
                    elem.phone =          columns[3];
                    elem.status =         columns[4];
                    elem.gender =         columns[5];
                    elem.dateOfBirth =    columns[6];

                    allUsersList.Add(elem);
                }
            }
            catch (Exception e) { ViewBag.UsersMsg = e.Message; }

            return View(allUsersList);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ChangeDealStatus()
        {

            return View();
        }

        // The following control recives a deals id and the status to 
        // to change the deal to. The number of status are limited to
        // those available in the view
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ChangeDealStatus(int id, string status)
        {
            // Validate for empty strings and null values
            if (status == "" || status == null || id <= 0) 
            {
                ViewBag.ChangeDealMsg = "empty or invalid entry";
                return View();
            }

            // Use the helper class to call the REST API with boolean flag
            // for sucess or failuare
            if (DealOperations.ChangeStatus(id, status)) 
                ViewBag.ChangeDealMsg = "Sucessful change!";
            else
                ViewBag.ChangeDealMsg = "Change did not happen!";

            return View();
        }


        [Authorize(Roles = "Admin")]
        public IActionResult ChangeUserStatus()
        {

            return View();
        }

        // The following control recives a users email and the status to 
        // to change the user to. The number of status are limited to
        // those available in the view
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ChangeUserStatus(string email, string status)
        {
            //Validate for empty strings and null values.
            if (status == "" || status == null || email =="" || email == null)
            {
                ViewBag.ChangeUserMsg = "empty or invalid entry";
                return View();
            }

            // Make a call to the userMgmt API using the helper class with flag
            // for the sucess of the call
            if (UsersOperations.changeStatus(email, status))
                ViewBag.ChangeUserMsg = "Sucessful change!";
            else
                ViewBag.ChangeUserMsg = "Change did not happen!";

            return View();
        }


        [Authorize(Roles = "Admin")]
        public IActionResult AddDeal()
        {

            return View();
        }

        // The following control recives a deals data to add it to deals
        // table. the data fields are validated before calling the 
        // DealMgmt REST API
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddDeal(string name,string descr,string status,int amount,string currency)
        {
            if (name == ""      || name == null     ||
                descr == ""     || descr == null    ||
                status == ""    || status == null   ||
                currency == ""  || currency == null   )
            
            {
                ViewBag.DealAddResponse = "Empty fields !";
                return View();
            }

            // Using helper class static method to call the DealMgmt API with boolean
            // to flag the sucess or failuare of the operation
            if (DealOperations.AddDeal(name, descr, status, amount, currency))
                ViewBag.DealAddResponse = "Added sucessfully !";
            else
                ViewBag.DealAddResponse = "Unsucessful add !";

            return View();
        }


        // The following control calls the DealMgmt REST API to get the 
        // number of claimed deals in a list and pass them to the view
        // so they can be grouped in table/grid view
        [Authorize(Roles = "Admin")]
        public IActionResult ClaimedDeals()
        {
            string ClaimedDeals = DealOperations.getAllClaimedDeals();
            var allDeals = new List<ClaimedDeals>();
            try
            {
                string[] rows = ClaimedDeals.Split('\n');

                foreach (var row in rows)
                {
                    ClaimedDeals dealElem = new ClaimedDeals();
                    string[] columns = row.Split(',');

                    for (int i = 0; i < columns.Length; i++)
                        columns[i] = columns[i].Trim();

                    dealElem.ID = Int32.Parse(columns[0]);
                    dealElem.UserId = Int32.Parse(columns[1]);
                    dealElem.DealID = Int32.Parse(columns[2]);
                    dealElem.Amount = Int32.Parse(columns[3]);
                    dealElem.Currency = columns[4];

                    allDeals.Add(dealElem);
                }
            }
            catch (Exception) {/*Consider using this for error handling */}

            return View(allDeals);
        }



        [Authorize(Roles = "Admin")]
        public IActionResult UserClaimedDeals()
        {
            return View();
        }
        
        // The following control calls the DealMgmt REST API to get the 
        // number of claimed deals for a certain user using their ID
        // grouping results in list and passing them as a list to assoicated view
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UserClaimedDeals(int ID)
        {
            string ClaimedDeals = DealOperations.getUserClaimedDeals(ID);
            var Deals = new List<ClaimedDeals>();
            try
            {
                string[] rows = ClaimedDeals.Split('\n');

                foreach (var row in rows)
                {
                    ClaimedDeals dealElem = new ClaimedDeals();
                    string[] columns = row.Split(',');

                    for (int i = 0; i < columns.Length; i++)
                        columns[i] = columns[i].Trim();

                    dealElem.ID = Int32.Parse(columns[0]);
                    dealElem.UserId = Int32.Parse(columns[1]);
                    dealElem.DealID = Int32.Parse(columns[2]);
                    dealElem.Amount = Int32.Parse(columns[3]);
                    dealElem.Currency = columns[4];

                    Deals.Add(dealElem);
                }
            }
            catch (Exception) {/*Consider using this for error handling */}

            return View(Deals);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUsers()
        {
            return View();
        }
        
        // The following control calls the UserMgmt REST API to delete the 
        // users using a set of emails seperated by a ','
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteUsers(string emailz)
        {
            if (emailz == "" || emailz == null)
            {
                ViewBag.DeleteResponse = "Empty fields";
                return View();
            }
            string[] email = emailz.Split(',');

            if (email == null)
            {
                if (!AccountController.IsValidEmail(emailz))
                {
                    ViewBag.DeleteResponse = "Invalid entry, check the email";
                    return View();
                }

                if (!UsersOperations.DeleteUser(emailz))
                {
                    ViewBag.DeleteResponse = "Failed to delete " + emailz;
                    return View();
                }

            }

            for (int i = 0; i < email.Length; i++)
            {
                if (!AccountController.IsValidEmail(email[i]))
                {
                    ViewBag.DeleteResponse = "Invalid entry, check the emails";
                    return View();
                }
            }

            for (int i = 0; i < email.Length; i++)
            {
                if (!UsersOperations.DeleteUser(email[i]))
                {
                    ViewBag.DeleteResponse = "Failed to delete " + email[i]
                                            + "Any emails entered prior to " + email[i]
                                            + " are deleted";
                    return View();
                }
            }

            ViewBag.DeleteResponse = "email(s) deleted sucessfully !";
            return View();
        }

        // This control returns a set of operations that a user can perform including
        // the view of their claimed deals, viewing of all deals and the ability to claim a deal
        [Authorize]
        public IActionResult UserProfile()
        {
            return View();
        }

        // The following control will make a call to the REST API deal mgmt 
        // to get all the claimed deals for the logged in user using their 
        // identity tickes and a helper function to get the deals
        [Authorize]
        public IActionResult MyClaimedDeals()
        {
            // REST API call to DealMgmt
            int userID = DealOperations.getUserID(User.Identity.Name);

            // DealMgmt API call to get all the claimed deals for a user using their ID
            string ClaimedDeals = DealOperations.getUserClaimedDeals(userID);

            // A list to pass to the assoicated view
            var Deals = new List<ClaimedDeals>();
            try
            {
                string[] rows = ClaimedDeals.Split('\n');

                foreach (var row in rows)
                {
                    ClaimedDeals dealElem = new ClaimedDeals();
                    string[] columns = row.Split(',');

                    for (int i = 0; i < columns.Length; i++)
                        columns[i] = columns[i].Trim();

                    dealElem.ID =     Int32.Parse(columns[0]);
                    dealElem.UserId = Int32.Parse(columns[1]);
                    dealElem.DealID = Int32.Parse(columns[2]);
                    dealElem.Amount = Int32.Parse(columns[3]);
                    dealElem.Currency =           columns[4];

                    Deals.Add(dealElem);
                }
            }
            catch (Exception) {/*Consider using this for error handling */}

            return View(Deals);
        }

        // This control gets all the available deals in the database
        // so alike users and admins could view the deals
        [Authorize]
        public IActionResult Deals()
        {
            // REST API call DealMgmt
            string Deals = DealOperations.getAllDeals();
            var allDeals = new List<Deal>();
            try
            {
                string[] rows = Deals.Split('\n');

                foreach (var row in rows)
                {
                    Deal dealElem = new Deal();
                    string[] columns = row.Split(',');

                    for (int i = 0; i < columns.Length; i++)
                        columns[i] = columns[i].Trim();

                    dealElem.ID = Int32.Parse(columns[0]);
                    dealElem.name = columns[1];
                    dealElem.description = columns[2];
                    dealElem.status = columns[3];
                    dealElem.amount = Int32.Parse(columns[4]);
                    dealElem.currency = columns[5];

                    allDeals.Add(dealElem);
                }
            }
            catch (Exception e) { ViewBag.DealsMsg = e.Message; }

            return View(allDeals);
        }

        [Authorize]
        public IActionResult ClaimDeal()
        {
            return View();
        }
        
        // Claim a deal using the DealMgmt API where the deal
        // ID is entered by user.
        [Authorize]
        [HttpPost]
        public IActionResult ClaimDeal(int DealID)
        {
            if (DealID <= 0)
            {
                ViewBag.ClaimDealMsg = "Invalid ID" + DealID;
                return View();
            }

            string deal = DealOperations.getDeal(DealID);

            string[] columns = deal.Split(',');

            for (int i = 0; i < columns.Length; i++)
                columns[i] = columns[i].Trim();

            int userID = DealOperations.getUserID(User.Identity.Name);
            int amount = Int32.Parse(columns[4]);
            string currency = columns[5];

            if (DealOperations.claimDeal(userID, DealID, amount, currency))
            {
                ViewBag.ClaimDealMsg = "Sucessfully claimed !";
                DealOperations.ChangeStatus(DealID, "Inactive");
            }
            else 
                ViewBag.ClaimDealMsg = "Unsucessful claim !";
            



            return View();
        }


        // The following control method will recieve a file 
        // from an upload form in the user profile view  where
        // it is validated as a picture, uploadded to wwwroot directory
        // and its name and path are stored in the database as a
        // pair of email and the absolute path for the stored image.
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            // Getting the user ID for the logged in email
            int userID = DealOperations.getUserID(User.Identity.Name);


            if (file != null && file.Length > 0)
            {
                string ext = Path.GetExtension(file.FileName).Trim().ToLower();

                // Validating that the file is an image
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".bmp")
                {
                    
                    ViewBag.UploadStatus = "Bad image format";
                    return View("UserProfile");
                }

                string name = userID.ToString() + ext;
                

                string filePath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot",
                    name);

                // Adding the picture info to the DB table
                UsersOperations.SetProfilePics(filePath, User.Identity.Name);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                ViewBag.fileName = name;
            }

            
            ViewBag.UploadStatus = "Uploaded sucessfully";
            return View("UserProfile");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
