using Application.Identity.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Models;
using WebApi.Identity.Requests;

namespace WebApi.Identity.Controllers;

[Produces("application/json")]
public class IdentityController : Controller
{
    private readonly IMediator _mediator;

    public IdentityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registers the user in the system and sends an email with link the confirmation email link (With email confirmation token and user id).
    /// </summary>
    /// <response code="200">User has been registered successfully.</response>
    /// <response code="400">User provided incorrect registration information.</response>
    [HttpPost("/identity/register-any-user")]
    [ProducesResponseType(typeof(RegisterAnyUser.RegisterAnyUserResponse), 200)]
    [ProducesResponseType(typeof(RegisterAnyUser.RegisterAnyUserResponse), 400)]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(new ModelStateErrorResponse(ModelState));
        }
        
        return Ok(await _mediator.Send(new RegisterAnyUser.RegisterAnyUserCommand(request.Email, request.Password)));
    }
    
    /// <summary>
    /// Logs user to the system
    /// </summary>
    /// <response code="200">User logged in successfully.</response>
    /// <response code="400">User provided incorrect login information.</response>
    [HttpPost("/identity/login")]
    [ProducesResponseType(typeof(Login.ApplicationLoginResponse), 200)]
    [ProducesResponseType(typeof(Login.ApplicationLoginResponse), 400)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return Ok(new ModelStateErrorResponse(ModelState));
        }
        
        return Ok(await _mediator.Send(new Login.ApplicationLoginCommand(request.Email, request.Password)));
    }
    
    /// <summary>
    /// Changes user's password
    /// </summary>
    /// <response code="200">Password changed successfully.</response>
    /// <response code="400">Some validation error occurred.</response>
    [HttpPost("/identity/reset-password")]
    [ProducesResponseType(typeof(ChangePassword.ChangePasswordResponse), 200)]
    [ProducesResponseType(typeof(ChangePassword.ChangePasswordResponse), 400)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        return Ok(await _mediator.Send(new ChangePassword.ChangePasswordCommand(request.UserId, request.NewPassword, request.Token)));
    }

    /// <summary>
    /// Sends an email with the change password request link (With change password token and user id).
    /// </summary>
    /// <response code="200">Email sent successfully.</response>
    /// <response code="400">Something went wrong during generating token.</response>
    [HttpPost("/identity/mail-change-password-token")]
    [ProducesResponseType(typeof(MailChangePasswordToken.MailChangePasswordTokenResponse), 200)]
    [ProducesResponseType(typeof(MailChangePasswordToken.MailChangePasswordTokenResponse), 400)]
    public async Task<IActionResult> MailChangePasswordToken([FromBody] MailChangePasswordTokenRequest request)
    {
        return Ok(await _mediator.Send(new MailChangePasswordToken.MailChangePasswordTokenCommand(request.Email)));
    }
    
    /// <summary>
    /// Sends an email with the email request link (With email confirmation token).
    /// </summary>
    /// <response code="200">Email sent successfully.</response>
    /// <response code="400">Something went wrong during generating token.</response>
    [HttpPost("/identity/mail-email-confirmation-token")]
    [ProducesResponseType(typeof(MailEmailConfirmation.MailEmailConfirmationResponse), 200)]
    [ProducesResponseType(typeof(MailEmailConfirmation.MailEmailConfirmationResponse), 400)]
    public async Task<IActionResult> MailEmailConfirmationToken([FromBody] MailChangePasswordTokenRequest request)
    {
        return Ok(await _mediator.Send(new MailEmailConfirmation.MailEmailConfirmationCommand(request.Email)));
    }
    
    /// <summary>
    /// Confirm user email
    /// </summary>
    /// <response code="200">Email was confirmed successfully.</response>
    /// <response code="400">Something went wrong token confirming.</response>
    [HttpPost("/identity/confirm-email")]
    [ProducesResponseType(typeof(ConfirmUserEmail.ConfirmUserEmailResponse), 200)]
    [ProducesResponseType(typeof(ConfirmUserEmail.ConfirmUserEmailResponse), 400)]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmUserEmailRequest request)
    {
        return Ok(await _mediator.Send(new ConfirmUserEmail.ConfirmUserEmailCommand(request.UserId, request.Token)));
    }
    
    /// <summary>
    /// Refreshes expired Jwt token.
    /// </summary>
    /// <response code="200">Token refreshed successfully.</response>
    /// <response code="400">Jwt token is incorrect, refresh token is incorrect, refresh token is invalidated or expired</response>
    [HttpPost("/identity/refresh-token")]
    [ProducesResponseType(typeof(RefreshToken.RefreshTokenCommand), 200)]
    [ProducesResponseType(typeof(RefreshToken.RefreshTokenCommand), 400)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        return Ok(await _mediator.Send(new RefreshToken.RefreshTokenCommand(request.Token, request.RefreshToken)));
    }
}