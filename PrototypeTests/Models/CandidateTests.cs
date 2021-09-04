using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Prototype.Models.Tests
{
    [TestClass()]
    public class CandidateTests
    {

        DateTime validDateInFuture;
        DateTime validDateInPast;
        Candidate candidate;
        double ratingValid;
        double ratingInvalidUpper;
        double ratingInvalidLower;
        double rateValid;
        double rateInvalid;


        public CandidateTests()
        {
            validDateInFuture = DateTime.Now.AddDays(1);
            validDateInPast = DateTime.Now.AddDays(-1);
            candidate = new Candidate();
            ratingValid = 3;
            ratingInvalidLower = 0;
            ratingInvalidUpper = 6;
            rateValid = 8.21;
            rateInvalid = 8.2;

        }
        [TestMethod()]
        public void TestSetAvailableDateInFuture()
        {
            candidate.AvailableFrom = validDateInFuture;

            Assert.AreEqual(candidate.AvailableFrom, validDateInFuture);
            Assert.AreEqual(candidate.IsAvailable, false);
        }

        [TestMethod()]
        public void TestSetAvailableDateInPast()
        {
            candidate.AvailableFrom = validDateInPast;

            Assert.AreEqual(candidate.AvailableFrom, validDateInPast);
            Assert.AreEqual(candidate.IsAvailable, true);
        }

        [TestMethod()]
        public void TestRatingValid()
        {
            candidate.Rating = ratingValid;

            Assert.AreEqual(candidate.Rating, ratingValid);

        }


        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRatingInValidLower()
        {
            candidate.Rating = ratingInvalidLower;


        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRatingInValidUpper()
        {
            candidate.Rating = ratingInvalidUpper;


        }

        [TestMethod()]
        public void TestRateValid()
        {
            candidate.Rate = rateValid;

            Assert.AreEqual(candidate.Rate, rateValid);

        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRateInValid()
        {
            candidate.Rate = rateInvalid;


        }
    }
}