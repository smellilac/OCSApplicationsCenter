public class ApplicationsModelValidator : AbstractValidator<ApplicationsModel>
{
    private readonly UsersContext _contextUsers;

    public ApplicationsModelValidator(UsersContext contextUsers)
    {
        _contextUsers = contextUsers;

        RuleFor(app => app.Author)
            .NotNull()
            .WithMessage("User is required for the application")
            .Must(BeGuid).WithMessage("UserId must be a valid Guid");

        RuleFor(app => app.Author)
            .MustAsync(BeUniqueUserId)
            .WithMessage("User with this Id already exists");
    }

    private bool BeGuid(Guid userId)
    {
       return Guid.TryParse(userId.ToString(), out _);
    }

    private async Task<bool> BeUniqueUserId(Guid userId, CancellationToken token)
    {
        var existingUser = await _contextUsers.Users.FirstOrDefaultAsync(u => u.Id == userId, token);
        return existingUser == null;
    }
}