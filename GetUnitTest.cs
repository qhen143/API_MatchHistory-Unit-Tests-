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
    public class GetUnitTest
    {
        public static readonly DbContextOptions<MatchHistoryContext> options
= new DbContextOptionsBuilder<MatchHistoryContext>()
.UseInMemoryDatabase(databaseName: "testDatabase2")
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

                MatchHistoryItem matchItem2 = new MatchHistoryItem()
                {
                    Date = "2018/02/02",
                    Location = "Cue City",
                    Game = "8 Ball",
                    Home = "Opponent 1",
                    Opposition = "Opponent 2",
                    Winner = "Opponent 1",
                    Comment = "N/A"
                };

                context.MatchHistoryItem.Add(matchItem1);
                context.MatchHistoryItem.Add(matchItem2);
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
        public async Task Test1_GetMatchHistoryItemById()
        {
            using (var context = new MatchHistoryContext(options))
            {
                //Given
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

                // When
                MatchHistoryController matchHistoryController = new MatchHistoryController(context);
                var result = await matchHistoryController.GetMatchHistoryItem(7) as OkObjectResult;

                // Then
                var actual = result.Value as API_MatchHistory.Models.MatchHistoryItem;
                CompareMatches(matchItem1, actual);
            }
        }

        [TestMethod]
        public async Task Test2_GetMatchHistoryItemById200StatusCode()
        {
            using (var context = new MatchHistoryContext(options))
            {
                // When
                MatchHistoryController matchHistoryController = new MatchHistoryController(context);
                var result = await matchHistoryController.GetMatchHistoryItem(10) as OkObjectResult;

                // Then
                var response = result.StatusCode;
                Assert.IsNotNull(response);
                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            }
        }

        [TestMethod]
        public async Task Test3_GetMatchHistoryItemById_InvalidId()
        {
            using (var context = new MatchHistoryContext(options))
            {
                // When
                MatchHistoryController matchHistoryController = new MatchHistoryController(context);
                var result = await matchHistoryController.GetMatchHistoryItem(420) as NotFoundResult;

                // Then
                var response = result.StatusCode;
                Assert.IsNotNull(response);
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }
        }

        [TestMethod]
        public void Test4_GetMatchHistoryItem200StatusCode()
        {
            using (var context = new MatchHistoryContext(options))
            {
                // When
                MatchHistoryController matchHistoryController = new MatchHistoryController(context);
                IActionResult result = matchHistoryController.GetMatchHistoryItem() as IActionResult;

                // Then
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void Test5_GetMatchHistoryItem()
        {
            using (var context = new MatchHistoryContext(options))
            {
                //Given
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

                MatchHistoryItem matchItem2 = new MatchHistoryItem()
                {
                    Date = "2018/02/02",
                    Location = "Cue City",
                    Game = "8 Ball",
                    Home = "Opponent 1",
                    Opposition = "Opponent 2",
                    Winner = "Opponent 1",
                    Comment = "N/A"
                };
                List<MatchHistoryItem> expected = new List<MatchHistoryItem>();
                expected.Add(matchItem1);
                expected.Add(matchItem2);

                // When
                MatchHistoryController matchHistoryController = new MatchHistoryController(context);
                List<MatchHistoryItem> result = matchHistoryController.GetMatchHistoryItem().ToList();

                // Then
                for (var i = 0; i < expected.Count; i++)
                {
                    CompareMatches(expected[i], result[expected.Count-1-i]);
                }
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
