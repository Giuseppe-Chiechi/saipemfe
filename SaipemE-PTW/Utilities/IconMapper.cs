using MudBlazor;

namespace SaipemE_PTW.Utilities
{
    public class IconMapper
    {
    public static readonly Dictionary<string, string> MaterialFilledIcons = new()
    {
        ["PlaylistAddCheck"] = Icons.Material.Filled.PlaylistAddCheck,
        ["AddBusiness"] = Icons.Material.Filled.AddBusiness,
        ["Accessibility"] = Icons.Material.Filled.Accessibility,
        ["AddBox"] = Icons.Material.Filled.AddBox,
        ["ReportGmailerrorred"] = Icons.Material.Filled.ReportGmailerrorred,
        ["PersonAddAlt1"] = Icons.Material.Filled.PersonAddAlt1,
        ["DeviceHub"] = Icons.Material.Filled.DeviceHub,
        ["ResetTv"] = Icons.Material.Filled.ResetTv,
        ["AssignmentInd"] = Icons.Material.Filled.AssignmentInd,
        ["SupervisedUserCircle"] = Icons.Material.Filled.SupervisedUserCircle,
        ["NotificationsActive"] = Icons.Material.Filled.NotificationsActive,
        ["MonitorHeart"] = Icons.Material.Filled.MonitorHeart,
        ["AddLink"] = Icons.Material.Filled.AccessTime,
        ["LocalFireDepartment"] = Icons.Material.Filled.LocalFireDepartment,

        ["SevereCold"] = Icons.Material.Filled.SevereCold,
        ["Gradient"] = Icons.Material.Filled.Gradient,
        ["UploadFile"] = Icons.Material.Filled.UploadFile,

        // ... aggiungi altre icone che usi
    };

    public static string GetFilledIcon(string? iconKey)
    {
        if (string.IsNullOrWhiteSpace(iconKey)) return string.Empty;
        return MaterialFilledIcons.TryGetValue(iconKey, out var icon) ? icon : string.Empty;
    }
}
}
