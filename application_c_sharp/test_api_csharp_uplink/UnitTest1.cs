using Microsoft.AspNetCore.Mvc;
using api_csharp_uplink.Controllers;

namespace test_api_csharp_uplink
{

    public class UnitTest1
    {

        [Fact]
        public void Test1()
        {
            PositionningController positionningController = new PositionningController(null);
            IActionResult actionResult = positionningController.timeBusToNextStation(0);
            Assert.Equal("4 mn", "5 mn");
        }
    }
}