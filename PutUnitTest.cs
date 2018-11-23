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

namespace UnitTestMatchHistory
{
    [TestClass]
    public class PutUnitTest
    {
        public static readonly DbContextOptions<MatchHistoryContext> options
= new DbContextOptionsBuilder<MatchHistoryContext>()
.UseInMemoryDatabase(databaseName: "testDatabase1")
.Options;
        public static IConfiguration configuration = null;
        public static readonly IList<string> matchHistoryDates = new List<string> { "2018/11/21", "2018/11/22" };

        [TestInitialize]
        public void SetupDb()
        {
            using (var context = new MatchHistoryContext(options))
            {
                MatchHistoryItem matchItem1 = new MatchHistoryItem()
                {
                    Date = matchHistoryDates[0]
                };

                MatchHistoryItem matchItem2 = new MatchHistoryItem()
                {
                    Date = matchHistoryDates[1]
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
        public async Task Test1_PutMatchHistoryItemNoContentStatusCode()
        {
            using (var context = new MatchHistoryContext(options))
            {
                // Given
                string date = "2019/5/5";
                MatchHistoryItem matchItem1 = context.MatchHistoryItem.Where(x => x.Date == matchHistoryDates[0]).Single();
                matchItem1.Date = date;

                // When
                MatchHistoryController matchHistoryController = new MatchHistoryController(context);
                IActionResult result = await matchHistoryController.PutMatchHistoryItem(matchItem1.Id, matchItem1) as IActionResult;

                // Then
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(NoContentResult));
            }
        }

        [TestMethod]
        public async Task Test2_PutMatchHistoryItemUpdate()
        {
            using (var context = new MatchHistoryContext(options))
            {
                // Given
                string date = "2019/5/5";
                MatchHistoryItem matchItem1 = context.MatchHistoryItem.Where(x => x.Date == matchHistoryDates[0]).Single();
                matchItem1.Date = date;

                // When
                MatchHistoryController matchHistoryController = new MatchHistoryController(context);
                IActionResult result = await matchHistoryController.PutMatchHistoryItem(matchItem1.Id, matchItem1) as IActionResult;

                // Then
                matchItem1 = context.MatchHistoryItem.Where(x => x.Date == date).Single();
                Assert.AreEqual(date, matchItem1.Date, "Correct field updated");
            }
        }

    }
}
