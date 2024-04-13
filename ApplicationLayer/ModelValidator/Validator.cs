public class MainModelValidator : AbstractValidator<ApplicationsModel>
{
    PostgreContext _context;

    public MainModelValidator(PostgreContext context)
    {
        _context = context;
        RuleFor(app => app.Activity)
            .NotNull()
            .WithMessage("Activity field is required");

        RuleFor(app => app.Activity)
            .IsInEnum()
            .WithMessage("Invalid activity type");
        
        RuleFor(app => app.Author)
            .NotNull()
            .WithMessage("User is required for the application")
            .Must(BeGuid).WithMessage("UserId must be a valid Guid")
            .When((app, context) => context.RootContextData.ContainsKey("ApplyAuthorValidation")); 
            
        RuleFor(app => app.Author)
                .Must(BeNonEmptyGuid)
                .WithMessage("User Id must be a non-empty Guid (not all zeros)")
                .When((app, context) => context.RootContextData.ContainsKey("ApplyAuthorValidation")); 

        RuleFor(app => app.Author)
            .MustAsync(BeUniqueUserId)
            .WithMessage("User with this Id already has draft application! You can have only one draft application!")
            .When((app, context) => context.RootContextData.ContainsKey("ApplyAuthorValidation")); 
        
        RuleFor(app => app.Name)
            .NotNull()
            .WithMessage("Name is required");
        
        RuleFor(app => app.Outline)
            .NotNull()
            .WithMessage("Outline is required");

        RuleFor(app => app.Description)
            .NotNull()
            .WithMessage("Description is required");

        RuleFor(app => app.Name)
            .MaximumLength(100)
            .WithMessage("Length must be less then 100 characters");

        RuleFor(app => app.Outline)
            .MaximumLength(1000)
            .WithMessage("Length must be less then 1000 characters");

        RuleFor(app => app.Description)
            .MaximumLength(300)
            .WithMessage("Length must be less then 300 characters");
        }
    private bool BeGuid(Guid userId)
    {
        return Guid.TryParse(userId.ToString(), out _);
    }

    private bool BeNonEmptyGuid(Guid userId)
    {
        return userId != Guid.Empty;
    }

    private async Task<bool> BeUniqueUserId(Guid userId, CancellationToken token)
    {
        var existingUser = await _context.UnSubmittedApps.FirstOrDefaultAsync(u => u.Author == userId, token);
        return existingUser == null;
    }
}