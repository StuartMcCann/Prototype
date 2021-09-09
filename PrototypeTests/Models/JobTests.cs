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
    public class JobTests
    {


        Employer employer;
        int employerId; 
        public JobTests()
        {
            employer = new Employer();
            employerId = 1; 
            employer.EmployerId = employerId; 
        }



        [TestMethod()]
        public void ContructorTest()
        {
            Job job = new Job();


            Assert.AreEqual(job.IsFilled, false);
            Assert.AreEqual(job.IsLive, true);
            Assert.AreEqual(job.IsUnderContract, false);


        }

        [TestMethod()]
        public void AddEmployerTest()
        {
            Job job = new Job();
            job.AddEmployer(employer);
            Assert.AreEqual(job.Employer, employer);
            Assert.AreEqual(job.EmployerRefId, employerId);

        }
    }
}