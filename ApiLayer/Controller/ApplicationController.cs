
[ApiController]
[Route("[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationHandler _handler;

    public ApplicationsController(IApplicationHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("/applications/{id}")]
    public async Task<IActionResult> GetApplicationById(Guid id)
    {
        try
        {
            var app = await _handler.GetApplicationByIdAsync(id);
            return Ok(app);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/applications")]
    public async Task<IActionResult> GetApplicationsSubmittedAfterDateAsync([FromQuery] DateTime? submittedAfter, 
        [FromQuery] DateTime? unsubmittedOlder)
    {
        if ((submittedAfter.HasValue && submittedAfter.Value.Kind != DateTimeKind.Utc)
        || (unsubmittedOlder.HasValue && unsubmittedOlder.Value.Kind != DateTimeKind.Utc))
        {
            return BadRequest("You entered the UTC time incorrectly");
        }
        if (submittedAfter.HasValue && unsubmittedOlder.HasValue)
        {
            return BadRequest("Both submittedAfter and unsubmittedOlder parameters cannot be provided at the same time");
        }
        try
        {
            if (submittedAfter.HasValue)
            {
                var applications = await _handler.
                    GetApplicationsSubmittedAfterDateAsync(submittedAfter.Value);
                return Ok(applications);
            }
            else if (unsubmittedOlder.HasValue)
            {
                var draftApplications = await _handler
                    .GetDraftApplicationsAfterDateAsync(unsubmittedOlder.Value);
                return Ok(draftApplications);
            }
        return BadRequest("Please set the submittedAfter or unsubmittedOlder parameters.");
        }
        catch (Exception ex)
        {
            return BadRequest("An error occurred: " + ex.Message);
        }
    }
    
    [HttpGet("/users/{userId}/currentapplication")]
    public async Task<IActionResult> GetDraftApplicationForUserAsync(Guid userId) 
    { 
        try 
        {
            var app = await _handler.GetDraftApplicationForUserAsync(userId);
            return Ok(app);
        }
        catch (InvalidOperationException InvalidEx)
        {
            return BadRequest(InvalidEx.Message);
        }
    }

    [HttpGet("/applications/{id}/submit")]
    public async Task<IActionResult> SubmitApplicationForReviewAsync(Guid id)
    {
        try
        {
            var app = await _handler.SubmitApplicationForReviewAsync(id);
            await _handler.SaveAsync();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/activities")]
    public async Task<IActionResult> GetActivityTypesAsync()
    {
        var activityTypes = await _handler.GetActivityTypesAsync();
        return Ok(activityTypes);
    }

    [HttpPost("/applications")]
    public async Task<IActionResult> CreateApplicationAsync([FromBody] UnSubmittedApplications app)
    {
        try
        {
            await _handler.CreateApplicationAsync(app);
        }
        catch (FieldAccessException LengthEx)
        {
            return BadRequest(LengthEx.Message);
        }
        catch (FluentValidation.ValidationException FluentEx)
        {
            return BadRequest(FluentEx.Message);
        }
        catch (InvalidOperationException InvalidEx)
        {
            return BadRequest(InvalidEx.Message);
        }
        await _handler.SaveAsync();
        return Ok(app);
    }

    [HttpPut("/applications/{id}")]
    public async Task<IActionResult> UpdateApplicationAsync([FromBody] UnSubmittedApplications app,
        [FromRoute] Guid id)
    {
        Guid requestId = id;
        try
        {
            var appFromDb = await _handler.UpdateApplicationAsync(app, requestId);
            await _handler.SaveAsync();
            return Ok(appFromDb);
        }
        catch (InvalidOperationException _)
        {
            try 
            {
                var appSubmitted = ToSubmitted.ToSubmittedApp(app);
                var appFromDb = await _handler.UpdateApplicationAsync(appSubmitted, requestId); 
                await _handler.SaveAsync();
                return Ok(appFromDb);
            }
            catch (InvalidOperationException InvalidEx)
            {
                return  BadRequest("Check application`s ID! There is no application with this ID!" + InvalidEx.Message);
            }
        }
        catch (Exception ex)
        {
            return BadRequest("Something gone wrond..." + ex.Message);
        }
    }

    [HttpDelete("/applications/{id}")]
    public async Task<IActionResult> DeleteApplicationAsync(Guid id)
    {
        try
        {
            await _handler.DeleteApplicationAsync(id);
            await _handler.SaveAsync();
            return Ok();
        }
        catch (InvalidOperationException InvalidEx)
        {
            return BadRequest(InvalidEx.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}