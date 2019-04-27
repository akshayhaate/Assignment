using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System.Collections.Generic;

namespace Assignment
{
    [TestClass]
    public class TestScenarios
    {
        static IWebDriver Driver;
        string RunningPath = AppDomain.CurrentDomain.BaseDirectory;

        [TestInitialize]
        public void Setup()
        {
            Driver = new ChromeDriver(RunningPath+"\\chromedriver_win32");
            Driver.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com/");
            Driver.Manage().Window.Maximize();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Driver.Quit();
        }
        #region Test Cases
        [TestMethod]
        public void TS_PIM_01()
        {
            try
            {
                Login("Admin", "admin123");
                if (ValidateLogin())
                {
                    Console.WriteLine("Welcome To OrangeHRM");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        [TestMethod]
        public void TS_PIM_02()
        {
            try
            {
                Login("Admin", "admin123");
                if (ValidateLogin())
                {
                    ClickOnTopMenu("Admin");
                    ClickOnTopSubMenu("Organization");
                    HoverOverElement("General Information");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        [TestMethod]
        public void TS_PIM_03()
        {
            try
            {
                Login("Admin", "admin123");
                if (ValidateLogin())
                {
                    ClickOnTopMenu("Admin");
                    ClickOnTopSubMenu("Organization");
                    HoverOverElement("General Information");
                    ClickEditOrSave("Edit");

                    //check fields are editable 
                    List<string> Fields = new List<string> { "Organization Name", "Tax ID", "Registration Number", "Fax", "Country", "Note" };
                    foreach (string field in Fields)
                    {
                        ValidateFieldDisableStatusOnGeneralInformationPage(field, "false");
                    }

                    ClickEditOrSave("Save");

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        [TestMethod]
        public void TS_PIM_04()
        {
            try
            {
                Login("Admin", "admin123");
                if (ValidateLogin())
                {
                    ClickOnTopMenu("PIM");
                    ClickOnTopSubMenu("Add Employee");
                    AddEmployeeDetails("Test", "User" + DateTime.Now.ToString("fff"));
                    //Get employee id from Add Employee page
                    string AEid = GetEmployeeIDFromAddEmpPage();
                    ClickSave();

                    //Get employee id from personal details page
                    string PDEid = GetEmployeeIDFromPersonalDetailsPage();

                    //Validate employee is created with same employee id
                    Assert.AreEqual(AEid,PDEid);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        [TestMethod]
        public void TS_PIM_05()
        {
            try
            {
                Login("Admin", "admin123");
                if (ValidateLogin())
                {
                    //Upload Profile Picture
                    List<string> ImagePaths =  new List<string> { RunningPath+"\\Images\\panda.jpg", RunningPath + "\\Images\\Panda.png", RunningPath + "\\Images\\Zipper.gif" };

                    foreach (string IPath in ImagePaths)
                    {
                        ClickOnTopMenu("PIM");
                        ClickOnTopSubMenu("Add Employee");
                        AddEmployeeDetails("Test", "User" + DateTime.Now.ToString("fff"));

                        //Get employee id from Add Employee page
                        string AEid = GetEmployeeIDFromAddEmpPage();
                        UploadImage(IPath);
                        ClickSave();

                        //Get employee id from personal details page
                        string PDEid = GetEmployeeIDFromPersonalDetailsPage();

                        //Validate employee is created with same employee id
                        Assert.AreEqual(AEid, PDEid);
                        
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        [TestMethod]
        public void TS_PIM_06()
        {
            try
            {
                Login("Admin", "admin123");
                if (ValidateLogin())
                {
                    ClickOnTopMenu("Dashboard");
                    Thread.Sleep(5000);
                    HoverOverGraph();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        #endregion


        #region Methods
        public static void Login(string uname, string pwrd)
        {
            //Enter username
            IWebElement Username = Driver.FindElement(By.Id("txtUsername"));
            Username.Clear();
            Username.SendKeys(uname);

            //Enter password
            IWebElement password = Driver.FindElement(By.Id("txtPassword"));
            password.Clear();
            password.SendKeys(pwrd);

            //Click Login
            IWebElement LoginBtn = Driver.FindElement(By.Id("btnLogin"));
            LoginBtn.Click();

        }

        public bool ValidateLogin()
        {
            IWebElement welcome = Driver.FindElement(By.Id("welcome"));
            //IWebElement ErrorSpan = Driver.FindElement(By.Id("spanMessage"));
            bool IsSuccess = false;
            if (!welcome.Displayed)
            {
                Console.WriteLine("Login Failed");
                IsSuccess = false;
            }
            else
            {
                Console.WriteLine("login Successful");
                IsSuccess = true;
            }
            return IsSuccess;
        }

        public void ClickOnTopMenu(string menu)
        {
            string id = string.Empty;
            switch (menu.ToLower())
            {
                case "admin":
                    id = "menu_admin_viewAdminModule";
                    break;
                case "dashboard":
                    id = "menu_dashboard_index";
                    break;

                case "pim":
                    id = "menu_pim_viewPimModule";
                    break;

            }

            IWebElement MenuItem = Driver.FindElement(By.Id(id));
            MenuItem.Click();
            Thread.Sleep(2000);
        }

        public static void ClickOnTopSubMenu(string submenu)
        {
            string id = string.Empty;
            switch (submenu.ToLower())
            {
                case "organization":
                    id = "menu_admin_Organization";
                    break;

                case "add employee":
                    id = "menu_pim_addEmployee";
                    break;

            }

            IWebElement SubMenuItem = Driver.FindElement(By.Id(id));
            SubMenuItem.Click();
            Thread.Sleep(2000);
        }

        public static void HoverOverElement(string elementname)
        {
            string id = string.Empty;
            switch (elementname.ToLower())
            {
                case "general information":
                    id = "menu_admin_viewOrganizationGeneralInformation";
                    break;

            }

            Actions element = new Actions(Driver);
            element.MoveToElement(Driver.FindElement(By.Id(id))).Click().Build().Perform();
        }
        public static void ValidateFieldDisableStatusOnGeneralInformationPage(string elementname, string expected)
        {
            string xpath = string.Empty;
            switch (elementname.ToLower())
            {
                case "organization name":
                    xpath = "//form[@id='frmGenInfo']/descendant::input[@id='organization_name']";
                    break;

                case "tax id":
                    xpath = "//form[@id='frmGenInfo']/descendant::input[@id='organization_taxId']";
                    break;

                case "number of employees":
                    xpath = "//span[@id='numOfEmployees']";
                    break;

                case "phone":
                    xpath = "//form[@id='frmGenInfo']/descendant::input[@id='organization_phone']";
                    break;

                case "fax":
                    xpath = "//form[@id='frmGenInfo']/descendant::input[@id='organization_fax']";
                    break;

                case "email":
                    xpath = "//form[@id='frmGenInfo']/descendant::input[@id='organization_email']";
                    break;

                case "address street 1":
                    xpath = "//form[@id='frmGenInfo']/descendant::input[@id='organization_street1']";
                    break;

                case "address street 2":
                    xpath = "//form[@id='frmGenInfo']/descendant::input[@id='organization_street2']";
                    break;

                case "city":
                    xpath = "//form[@id='frmGenInfo']/descendant::input[@id='organization_city']";
                    break;

                case "state":
                    xpath = "//form[@id='frmGenInfo']/descendant::input[@id='organization_province']";
                    break;

                case "zip":
                    xpath = "//form[@id='frmGenInfo']/descendant::input[@id='organization_zipCode']";
                    break;

                case "country":
                    xpath = "//select[@id='organization_country']";
                    break;

                case "note":
                    xpath = "//textarea[@id='organization_note']";
                    break;

                case "registration number":
                    xpath = "//form[@id='frmGenInfo']/descendant::input[@id='organization_registraionNumber']";
                    break;
            }

            IWebElement Element = Driver.FindElement(By.XPath(xpath));
            string actual = Element.GetAttribute("disabled");
            if (actual == null)
            {
                actual = "false";
                Thread.Sleep(1000);
            }

            Assert.AreEqual(expected, actual);
        }
        public static void ClickEditOrSave(string option)
        {
            if (option.Equals("Edit"))
            {
                IWebElement EditBtn = Driver.FindElement(By.Id("btnSaveGenInfo"));
                EditBtn.Click();
            }

            else if (option.Equals("Save"))
            {
                IWebElement SaveBtn = Driver.FindElement(By.Id("btnSaveGenInfo"));
                SaveBtn.Click();
            }
        }

        public static void AddEmployeeDetails(string fname, string lname)
        {
            IWebElement Fname = Driver.FindElement(By.Id("firstName"));
            Fname.SendKeys(fname);

            IWebElement Lname = Driver.FindElement(By.Id("lastName"));
            Lname.SendKeys(lname);

        }

        public static void ClickSave()
        {
            Driver.FindElement(By.Id("btnSave")).Click();
        }


        public string GetCurrentUrl()
        {
            return Driver.Url;
        }

        public string GetEmployeeIDFromAddEmpPage()
        {
            IWebElement eid = Driver.FindElement(By.Id("employeeId"));
            return eid.GetAttribute("value");
        }

        public string GetEmployeeIDFromPersonalDetailsPage()
        {
            IWebElement eid = Driver.FindElement(By.Id("personal_txtEmployeeId"));
            return eid.GetAttribute("value");
        }

        public static void UploadImage(string Path)
        {
            IWebElement upload = Driver.FindElement(By.XPath("//input[@id='photofile']"));
            upload.SendKeys(Path);
        }


        public static void HoverOverGraph()
        {
            IList<IWebElement> PieElements = Driver.FindElements(By.XPath("//div[@id='div_graph_display_emp_distribution']/descendant::span"));
            foreach (IWebElement pie in PieElements)
            {
                string id = pie.GetAttribute("id");

                Actions element = new Actions(Driver);
                element.MoveToElement(Driver.FindElement(By.XPath("//span[@id='" + id + "']"))).Click().Build().Perform();
                Thread.Sleep(2000);
            }
        }
        #endregion
    }
}
