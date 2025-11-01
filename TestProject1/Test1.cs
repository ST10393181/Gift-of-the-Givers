using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Controllers;
using WebApplication8.Data;
using WebApplication8.Models;

namespace TestProject1
{
    [TestClass]
    public class ReliefProjectsControllerTests
    {
        private GiftOfGiversContext _context;
        private ReliefProjectsController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Initialize In-Memory Database
            var options = new DbContextOptionsBuilder<GiftOfGiversContext>()
                .UseInMemoryDatabase(databaseName: "ReliefProjectTestDb")
                .Options;

            _context = new GiftOfGiversContext(options);

            // Seed database
            var user = new User { UserID = 1, Email = "Bonno@gmail.com",Role="User",PasswordHash="",LastLogin=DateTime.Now };
            var reliefProject = new ReliefProject
            {
                ReliefProjectID = 1,
                ProjectName = "Water Relief Program",
                Description="Water Relief Program",
                Location = "Location A",
                StartDate = DateTime.Now,
                Status = "Active",
                EndDate = null,
                UserID = 1
            };

            _context.Users.Add(user);
            _context.ReliefProjects.Add(reliefProject);
            _context.SaveChanges();

            // Create controller instance
            _controller = new ReliefProjectsController(_context,null);
        }

        [TestMethod]
        public async Task Edit_NullId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Edit(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "Expected NotFoundResult when ID is null");
        }

        [TestMethod]
        public async Task Edit_IdNotFound_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Edit(99); // ID that doesn't exist

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "Expected NotFoundResult when project not found");
        }

        [TestMethod]
        public async Task Edit_ValidId_ReturnsViewResult_WithCorrectModel()
        {
            // Act
            var result = await _controller.Edit(1);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult, "Expected a ViewResult");

            var model = viewResult.Model as ReliefProject;
            Assert.IsNotNull(model, "Expected model to be of type ReliefProject");
            Assert.AreEqual(1, model.ReliefProjectID, "Model should have correct ReliefProjectID");
            Assert.AreEqual("Water Relief Program", model.ProjectName, "Model should have correct ProjectName");

            // Assert ViewData
            Assert.IsTrue(viewResult.ViewData.ContainsKey("UserID"), "Expected ViewData to contain UserID");
            var selectList = viewResult.ViewData["UserID"] as SelectList;
            Assert.IsNotNull(selectList, "Expected UserID ViewData to be a SelectList");
        }
    }
}
