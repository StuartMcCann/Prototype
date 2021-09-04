using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.Models.Tests
{
    [TestClass()]
    public class ApplicationUserTests
    {
        string validFirstName;
        string validLastName;
        int validEmployerId;
        Employer validEmployer; 

        public ApplicationUserTests()
        {
            validFirstName = "ValidFirstName";
            validLastName = "ValidLastName";
            validEmployerId = 1;
            validEmployer = new Employer(); 
        }


        [TestMethod()]
        public void FirstNameTest()
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.FirstName = validFirstName;
            Assert.AreEqual(applicationUser.FirstName, validFirstName);
        }


        [TestMethod()]
        public void LastNameTest()
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.LastName = validLastName;
            Assert.AreEqual(applicationUser.LastName, validLastName);
        }

        [TestMethod()]
        public void EmployerIdTest()
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.EmployerId = validEmployerId;
            Assert.AreEqual(applicationUser.EmployerId, validEmployerId);
        }

        [TestMethod()]
        public void EmployerTest()
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.Employer = validEmployer;
                
              
            Assert.AreEqual(applicationUser.Employer, validEmployer);
        }
    }


}