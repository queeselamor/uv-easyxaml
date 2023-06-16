namespace Kropka.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;

public interface IConverterItemOwner
{
    #region Properties
    public bool CanUpdateStates { get; set; }
    #endregion

    #region Methods
    void UpdateStates();
    #endregion
}