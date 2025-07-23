using BmsIngest.Models;
using BmsIngest.Services.BmsRetrieval;
using Microsoft.AspNetCore.Mvc;

namespace BmsIngest.Controllers;

[Route("api/Test")]
[ApiController]
public class TestController : ControllerBase
{

    private readonly IBmsRetrievalService _bmsRetrieval;

    public TestController(IBmsRetrievalService bmsRetrieval)
    {
        _bmsRetrieval = bmsRetrieval;
    }
    
    [HttpPost("GetInformation")]
    public async Task<ActionResult<Information?>> GetInformation()
    {
        return await _bmsRetrieval.GetInformation();
    }
}