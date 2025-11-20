using Microsoft.AspNetCore.Components;
using MudBlazor;
using SaipemE_PTW.Components.Base;
using SaipemE_PTW.Services.Common;
using SaipemE_PTW.Shared.Models.Auth;
using SaipemE_PTW.Shared.Models.Menu;

namespace SaipemE_PTW.Components.Common
{
    public partial class Menu : CommonComponentBase
    {
        [Inject] private IMenuService menu { get; set; } = default!;

        private int Columns = 4;
        List<MenuCard>? Cards;


        protected override async Task OnInitializedCoreAsync()
        {
            if (userRole.Equals(UserRole.Anonymous))
            {
                Navigation.NavigateTo("/login", forceLoad: true);
                return;
            }

            await onload();

            if (Cards == null)
            {
                Cards = new List<MenuCard>();
                return;
            }

            switch (userRole)
            {
                case UserRole.AutoritaRichiedente:
                    Cards = Cards.Where(c => c.id != 10).ToList();
                    break;
                case UserRole.AutoritaEsecutrice:
                    Cards = Cards.Select(card => new MenuCard
                    {
                        id = card.id,
                        Title = card.Title,
                        Icon = card.Icon,
                        Sections = card.Sections?.Select(section => new MenuSection
                        {
                            id = section.id,
                            Header = section.Header,
                            Items = section.Items?
                                .Where(item => item.id != 18 && item.id != 19 && item.id != 20)
                                .ToList()
                        }).ToList()
                    })
                    .Where(c => c.id != 10).ToList();

                    break;
                case UserRole.AutoritaEmittente:
                    Cards = Cards.Select(card => new MenuCard
                    {
                        id = card.id,
                        Title = card.Title,
                        Icon = card.Icon,
                        Sections = card.Sections?.Select(section => new MenuSection
                        {
                            id = section.id,
                            Header = section.Header,
                            Items = section.Items?
                                .Where(item => item.id != 18 && item.id != 19 && item.id != 20)
                                .ToList()
                        }).ToList()
                    })
                    .Where(c => c.id != 10)
                    .ToList();
                    break;
                case UserRole.PTWCoordinator:
                    Cards = Cards.Select(card => new MenuCard
                    {
                        id = card.id,
                        Title = card.Title,
                        Icon = card.Icon,
                        Sections = card.Sections?.Select(section => new MenuSection
                        {
                            id = section.id,
                            Header = section.Header,
                            Items = section.Items?
                                .Where(item => item.id != 18 && item.id != 19 && item.id != 20)
                                .ToList()
                        }).ToList()
                    })
                     .Where(c => c.id != 10)
                     .ToList();
                    break;
                case UserRole.CoordinatoreInEsecuzioneCSE:
                    Cards = Cards.Select(card => new MenuCard
                    {
                        id = card.id,
                        Title = card.Title,
                        Icon = card.Icon,
                        Sections = card.Sections?.Select(section => new MenuSection
                        {
                            id = section.id,
                            Header = section.Header,
                            Items = section.Items?
                                .Where(item => item.id != 18 && item.id != 19 && item.id != 20)
                                .ToList()
                        }).ToList()
                    })
                    .Where(c => c.id != 10)
                    .ToList();
                    break;
                case UserRole.PersonaAutorizzataTestGas:
                    Cards = Cards.Select(card => new MenuCard
                    {
                        id = card.id,
                        Title = card.Title,
                        Icon = card.Icon,
                        Sections = card.Sections?.Select(section => new MenuSection
                        {
                            id = section.id,
                            Header = section.Header,
                            Items = section.Items?
                                .Where(item => item.id != 18 && item.id != 19 && item.id != 20)
                                .ToList()
                        }).ToList()
                    })
                    .Where(c => c.id != 10)
                    .ToList();
                    break;
                case UserRole.AutoritaOperativa:
                    Cards = Cards.Where(c => c.id != 10).ToList();
                    break;
                case UserRole.AmministratoreSistema:
                    Columns = 3;
                    Cards = Cards.Select(card => new MenuCard
                    {
                        id = card.id,
                        Title = card.Title,
                        Icon = card.Icon,
                        Sections = card.Sections?.Select(section => new MenuSection
                        {
                            id = section.id,
                            Header = section.Header,
                            Items = section.Items?
                                .Where(item => item.id != 18 && item.id != 19 && item.id != 20)
                                .ToList()
                        }).ToList()
                    }).ToList();

                    break;
                case UserRole.SuperOwner:
                    Columns = 3;
                    break;
                case UserRole.Visitatore:

                    Cards = Cards.Where(c => c.id != 10).ToList();
                    break;
                default:

                    break;

            }
        }

        protected async Task onload()
        {
            try
            {
                Cards = await menu.GetMenuAsync(LanguageService.GetCurrentCulture());
            }
            catch
            {
                Cards = new List<MenuCard>();
            }
        }
    }
}
