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
    public class EnumExtensionsTests
    {

        JobTitle jobTitle;
        string displayName; 

        public EnumExtensionsTests()
        {
            jobTitle = JobTitle.SoftwareEngineer;

            displayName = "Software Engineer"; 

        }

        [TestMethod()]
        public void GetDisplayNameTest()
        {
            Assert.AreEqual(jobTitle.GetDisplayName(), displayName); 
        }
    }
}