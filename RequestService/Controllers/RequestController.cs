using Microsoft.AspNetCore.Mvc;
using RequestService.Policies;

namespace RequestService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly ClientPolicy _policy;
        private readonly IHttpClientFactory _clientFactory;

        public RequestController(ClientPolicy policy, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _policy = policy;
        }

        //GET api/request

        [HttpGet]
        public async Task<ActionResult> MakeRequest()
        {
            //var client = new HttpClient();

            var client = _clientFactory.CreateClient();

            //var response = await client.GetAsync("https://localhost:7108/api/response/11");

            var response = await _policy
                .ExponentialHttpRetry
                .ExecuteAsync(() => client.GetAsync("https://localhost:7108/api/response/11"));

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("=--> ResponseService returned SUCCESS");
                return Ok();
            }

            Console.WriteLine("=--> ResponseService returned FAILURE");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}