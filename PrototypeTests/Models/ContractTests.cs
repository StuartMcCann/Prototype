using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prototype.Data;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.Models.Tests
{
    [TestClass()]
    public class ContractTests
    {

        double agreedRateValid;
        double agreedRateInvalid;
        int ratingByCandidate;
        int ratingByEmployer;
        Contract contract;
        DateTime endDate; 
      
 

        public ContractTests()
        {
            agreedRateValid = 8.21;
            agreedRateInvalid  = 8.2; 
            contract= new Contract();
            ratingByCandidate = 3;
            ratingByEmployer = 3;
            endDate = DateTime.Now; 
          
        }

        [TestMethod()]
        public void TestRateValid()
        {
            contract.AgreedRate = agreedRateValid;

            Assert.AreEqual(contract.AgreedRate, agreedRateValid);

        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRateInValid()
        {
            contract.AgreedRate = agreedRateInvalid;


        }

        [TestMethod()]
        public void TestRaingByCandidate()
        {
            contract.ContractRatingByCandidate = ratingByCandidate;

            Assert.AreEqual(contract.ContractRatingByCandidate, ratingByCandidate);
            Assert.AreEqual(contract.IsRatedByCandidate, true); 

        }

        [TestMethod()]
        public void TestRaingByEmployer()
        {
            contract.ContractRatingByEmployer = ratingByEmployer;

            Assert.AreEqual(contract.ContractRatingByEmployer, ratingByEmployer);
            Assert.AreEqual(contract.IsRatedByEmployer, true);

        }



        [TestMethod()]
        public void ContractContructorTest()
        {
            Contract contract = new Contract();

            Assert.AreEqual(contract.IsUnderContract, true); 
        }

        [TestMethod()]
        public void EndContractTest()
        {
            Contract contract = new Contract();
            contract.EndContract(endDate);

            Assert.AreEqual(contract.IsUnderContract, false);
            Assert.AreEqual(contract.EndDate, endDate);

        }


    }
}