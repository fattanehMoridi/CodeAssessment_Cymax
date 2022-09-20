
using Moq;
using RequestingAPIs.Services;
using System.Threading.Tasks;
using Xunit;

namespace RequestingAPIs.Tests
{

    public class RequestAPIsTest
    {
        private Mock<IHttpClientService> _httpClientService;
        private readonly GetOffer _sut;
        public RequestAPIsTest()
        {
            _httpClientService = new Mock<IHttpClientService>();
            InputRequest request = new InputRequest("ContactAddress", "WarehouseAddress", new double[] { 1.0, 2.0, 3.0 });
            _sut = new GetOffer(_httpClientService.Object, request);
        }

        [Fact]
        public async Task FirstApiTest()
        {
            _httpClientService.Setup(o => o.CallApiAsync(It.Is<RequestParam>(r =>
            r.Uri == "uri1" &&
            r.Query == $@"{{'Contact Address':{_sut.RequestQuery.SurceAddress},
                            'Warehouse Address':{_sut.RequestQuery.DestinationAddress},
                            'Package Dimensions':[{_sut.RequestQuery.CartonDimensions[0]},{_sut.RequestQuery.CartonDimensions[1]},{_sut.RequestQuery.CartonDimensions[2]}]}}" &&
            r.Type == "application/json" &&
            r.UserName == "UserName1" &&
            r.PassWord == "PassWord1"
            ))).Returns(Task.FromResult("{total:10}"));

            var resultFirst = await _sut.FirstOffer();
            Assert.Equal(10, resultFirst);
        }

        [Fact]
        public async Task SecondApiTest()
        {
            _httpClientService.Setup(o => o.CallApiAsync(It.Is<RequestParam>(r =>
           r.Uri == "uri2" &&
           r.Query == $"{{'consignee':{_sut.RequestQuery.SurceAddress},'consignor':{_sut.RequestQuery.DestinationAddress},'cartons':[{_sut.RequestQuery.CartonDimensions[0]},{_sut.RequestQuery.CartonDimensions[1]},{_sut.RequestQuery.CartonDimensions[2]}]}}" &&
           r.Type == "application/json" &&
           r.UserName == "UserName2" &&
           r.PassWord == "PassWord2"
           ))).Returns(Task.FromResult("{amount:10}"));

            var resultSecond = await _sut.SecondOffer();
            Assert.Equal(10, resultSecond);
        }

        [Fact]
        public async Task ThirdApiTest()
        {           
            _httpClientService.Setup(o => o.CallApiAsync(It.Is<RequestParam>(r =>
          r.Uri == "uri3" &&
          r.Query == $@"<xml>
                            < source > {_sut.RequestQuery.SurceAddress} </ source >
                            < destination > {_sut.RequestQuery.DestinationAddress} </ destination >
                            < packages >
                                < package > {_sut.RequestQuery.CartonDimensions[0]} </ package >
                                < package > {_sut.RequestQuery.CartonDimensions[1]} </ package >
                                < package > {_sut.RequestQuery.CartonDimensions[2]} </ package >
                            </ packages >
                            </ xml > " &&
          r.Type == "text / xml" &&
          r.UserName == "UserName3" &&
          r.PassWord == "PassWord3"
          ))).Returns(Task.FromResult("<xml><quote>10</quote></xml>"));
            var resultThird = await _sut.ThirdOffer();
            Assert.Equal(10, resultThird);
        }
    }

}