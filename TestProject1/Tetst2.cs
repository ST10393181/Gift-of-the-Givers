using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication8.Controllers;
using WebApplication8.Data;
using WebApplication8.Models;

namespace TestProject1
{
    [TestClass]
    public class DonationsControllerTests
    {
        private GiftOfGiversContext _context;
        private DonationsController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Create an in-memory database for testing
            var options = new DbContextOptionsBuilder<GiftOfGiversContext>()
                .UseInMemoryDatabase(databaseName: "DonationsTestDb")
                .Options;

            _context = new GiftOfGiversContext(options);

            // Seed mock data
            var disaster = new Disaster { Disaster_ID = 1, Name = "Flood Relief",Location="Location B",Description="Flooding",Status="Active" };
            var resource = new Resource { Resource_ID = 1, Name = "Water Bottles",Quantity=0,StorageLocation="Location B",Type="Food",LastUpdated=DateTime.Now };
            var donation = new Donation
            {
                Donation_ID = 1,
                DonorName= "Bonno",
                Amount = 1000,
                Disaster_ID = 1,
                Resource_ID = 1
            };

            _context.Disasters.Add(disaster);
            _context.Resources.Add(resource);
            _context.Donations.Add(donation);
            _context.SaveChanges();

            _controller = new DonationsController(_context,null);
        }

        [TestMethod]
        public async Task Edit_NullId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Edit(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult),
                "Expected NotFoundResult when ID is null");
        }

        [TestMethod]
        public async Task Edit_IdNotFound_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Edit(4); // ID that doesn't exist

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult),
                "Expected NotFoundResult when donation not found");
        }

        [TestMethod]
        public async Task Edit_ValidId_ReturnsViewResult_WithDonationModel()
        {
            // Act
            var result = await _controller.Edit(1);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult, "Expected a ViewResult");

            var model = viewResult.Model as Donation;
            Assert.IsNotNull(model, "Expected model to be of type Donation");
            Assert.AreEqual(1, model.Donation_ID, "Model should have correct Donation_ID");

            // Check ViewData
            Assert.IsTrue(viewResult.ViewData.ContainsKey("Disaster_ID"),
                "Expected ViewData to contain Disaster_ID");
            Assert.IsTrue(viewResult.ViewData.ContainsKey("Resource_ID"),
                "Expected ViewData to contain Resource_ID");

            // Check SelectList types
            Assert.IsInstanceOfType(viewResult.ViewData["Disaster_ID"], typeof(SelectList),
                "Disaster_ID ViewData should be a SelectList");
            Assert.IsInstanceOfType(viewResult.ViewData["Resource_ID"], typeof(SelectList),
                "Resource_ID ViewData should be a SelectList");
        }
    }
}
