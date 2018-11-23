using API_MatchHistory.Models;
using API_MatchHistory.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;

namespace UnitTestMatchHistory
{
    [TestClass]
    public class PostUnitTest
    {
        public static readonly DbContextOptions<MatchHistoryContext> options
= new DbContextOptionsBuilder<MatchHistoryContext>()
.UseInMemoryDatabase(databaseName: "testDatabase4")
.Options;
        public static IConfiguration configuration = null;

        [TestInitialize]
        public void SetupDb()
        {
            using (var context = new MatchHistoryContext(options))
            {
                MatchHistoryItem matchItem1 = new MatchHistoryItem()
                {
                    Date = "2018/01/01",
                    Location = "Cue City",
                    Game = "8 Ball",
                    Home = "Opponent 1",
                    Opposition = "Opponent 2",
                    Winner = "Opponent 1",
                    Comment = "N/A"
                };

                context.MatchHistoryItem.Add(matchItem1);
                context.SaveChanges();
            }
        }

        [TestCleanup]
        public void ClearDb()
        {
            using (var context = new MatchHistoryContext(options))
            {
                context.MatchHistoryItem.RemoveRange(context.MatchHistoryItem);
                context.SaveChanges();
            };
        }

        [TestMethod]
        public async Task Test1_PostMatchHistoryItem()
        {
            using (var context = new MatchHistoryContext(options))
            {
                //Given
                MatchHistoryItem matchItem1 = new MatchHistoryItem()
                {
                    Date = "2018/02/02",
                    Location = "Cue City",
                    Game = "8 Ball",
                    Home = "Opponent 1",
                    Opposition = "Opponent 2",
                    Winner = "Opponent 1",
                    Comment = "N/A"
                };

                // When
                MatchHistoryController matchHistoryController = new MatchHistoryController(context);
                var result = await matchHistoryController.PostMatchHistoryItem(matchItem1) as CreatedAtActionResult;

                // Then
                var actual = result.Value as API_MatchHistory.Models.MatchHistoryItem;
                CompareMatches(matchItem1, actual);
            }
        }

        [TestMethod]
        public async Task Test2_PostMatchHistoryItemCreatedStatusCode()
        {
            using (var context = new MatchHistoryContext(options))
            {
                //Given
                MatchHistoryItem matchItem1 = new MatchHistoryItem()
                {
                    Date = "2018/02/02",
                    Location = "Cue City",
                    Game = "8 Ball",
                    Home = "Opponent 1",
                    Opposition = "Opponent 2",
                    Winner = "Opponent 1",
                    Comment = "N/A"
                };

                // When
                MatchHistoryController matchHistoryController = new MatchHistoryController(context);
                var result = await matchHistoryController.PostMatchHistoryItem(matchItem1) as CreatedAtActionResult;

                // Then
                var response = result.StatusCode;
                Assert.IsNotNull(response);
                Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            }
        }

        private void CompareMatches(MatchHistoryItem expected, MatchHistoryItem result)
        {

            Assert.AreEqual(expected.Date, result.Date);
            Assert.AreEqual(expected.Home, result.Home);
            Assert.AreEqual(expected.Location, result.Location);
            Assert.AreEqual(expected.Opposition, result.Opposition);
            Assert.AreEqual(expected.Winner, result.Winner);
            Assert.AreEqual(expected.Comment, result.Comment);

        }
    }
}
