﻿using Blazorise;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace eVoucher;
public partial class App : ComponentBase
{
    private Theme theme = new Theme
    {
        BarOptions = new ThemeBarOptions
        {
            LightColors = new ThemeBarColorOptions
            {
                BackgroundColor = "#ff0000",
                Color = "#000000",
                ItemColorOptions = new ThemeBarItemColorOptions
                {
                    ActiveBackgroundColor = "#EDE7F6",
                    ActiveColor = "#000000",
                    HoverBackgroundColor = "#EDE7F6",
                    HoverColor = "#000000",
                },
            },
            DarkColors = new ThemeBarColorOptions
            {
                BackgroundColor = "#ff0000",
                Color = "#000000",
                ItemColorOptions = new ThemeBarItemColorOptions
                {
                    ActiveBackgroundColor = "#EDE7F6",
                    ActiveColor = "#000000",
                    HoverBackgroundColor = "#EDE7F6",
                    HoverColor = "#000000",
                },
            },
        },

        ColorOptions = new ThemeColorOptions
        {
            Primary = "#4527A0",
            Secondary = "#5E35B1",
            Success = "#00C853",
            Info = "#B9F6CA",
            Warning = "#FFC107",
            Danger = "#C62828",
        },
        BackgroundOptions = new ThemeBackgroundOptions
        {
            Primary = "#4527A0",
        },
        TextColorOptions = new ThemeTextColorOptions
        {

        },
        InputOptions = new ThemeInputOptions
        {
            CheckColor = "#4527A0",
        },
    };

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}