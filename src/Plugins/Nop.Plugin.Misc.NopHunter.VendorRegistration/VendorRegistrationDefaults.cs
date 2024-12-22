namespace Nop.Plugin.Misc.VendorRegistration;

/// <summary>
/// Represents plugin constants
/// </summary>
public static class VendorRegistrationDefaults
{
    public static string SystemName => "Misc.NopHunter.VendorRegistration";
    public static string ConfigurationRouteName => "Plugin.Misc.NopHunter.VendorRegistration.Configure";
    public static string CustomerAddedSuccessSessionKey => "_customerAddedSuccessSessionKey_";

    //Message
    public static string NEW_VENDOR_ACCOUNT_APPLY_STORE_VENDOR_NOTIFICATION => "Plugin.Misc.NopHunter.VendorRegistration.VendorNotificationAfterAccountCreation";
    public static string VENDOR_ACCOUNT_ACTIVATION_NOTIFICATION => "Plugin.Misc.NopHunter.VendorRegistration.VendorAccountActivationNotification";

    
}