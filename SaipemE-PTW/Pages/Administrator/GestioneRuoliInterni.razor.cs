using MudBlazor;
using SaipemE_PTW.Components.Base;

namespace SaipemE_PTW.Pages.Amministrazione
{
    public partial class RuoliInerni : CommonComponentBase
    {
        protected override async Task OnInitializedCoreAsync()
        {

            try
            {
                SetBreadcrumb(
                     new BreadcrumbItem("⬅ Back", href: "javascript:history.back()"),
                     new BreadcrumbItem(Localization.GetString("Home.Dashboard"), href: "/"),
                     new BreadcrumbItem("Ruoli Interni", href: null, disabled: true)
                 );


            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);


            }
            finally
            {
                await Task.CompletedTask;
            }
        }
    }
}
