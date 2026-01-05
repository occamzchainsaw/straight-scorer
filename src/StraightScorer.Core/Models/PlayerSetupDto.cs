using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StraightScorer.Core.Models;

public partial class PlayerSetupDto(Func<int> getTargetScore) : ObservableValidator
{
    private readonly Func<int> _getTargetScore = getTargetScore;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [StringLength(25, MinimumLength = 1)]
    string _name = "";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [CustomValidation(typeof(PlayerSetupDto), nameof(ValidateHeadStart))]
    int _headStart;

    [ObservableProperty] bool _isStarting;

    [ObservableProperty] bool _isNameValid = true;
    [ObservableProperty] bool _isHeadStartValid = true;

    public void RefreshValidation()
    {
        ValidateAllProperties();
    }

    public static ValidationResult? ValidateHeadStart(int headStart, ValidationContext context)
    {
        var instance = (PlayerSetupDto)context.ObjectInstance;
        int max = instance._getTargetScore();

        if (headStart < 0 || headStart >= max)
            return new ValidationResult("Invalid");

        return ValidationResult.Success;
    }

    // will send a message to a parent class, so that it can make sure only one player has this property set to true
    partial void OnIsStartingChanged(bool value)
    {
        if (value)
            WeakReferenceMessenger.Default.Send(new IsStartingPlayerChangedMessage(this));
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        // Update our boolean "flags" whenever the property changes
        if (e.PropertyName == nameof(Name))
        {
            IsNameValid = !GetErrors(nameof(Name)).Any();
        }
        else if (e.PropertyName == nameof(HeadStart))
        {
            IsHeadStartValid = !GetErrors(nameof(HeadStart)).Any();
        }

        if (e.PropertyName != nameof(HasErrors))
            WeakReferenceMessenger.Default.Send(new ValidationChangedMessage());
    }
}

public class IsStartingPlayerChangedMessage(PlayerSetupDto value) : ValueChangedMessage<PlayerSetupDto>(value);

public record ValidationChangedMessage();
