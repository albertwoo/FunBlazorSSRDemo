#nowarn "0020"

open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection

let builder = WebApplication.CreateBuilder(Environment.GetCommandLineArgs())

builder.Services.AddRazorComponents()
builder.Services.AddFunBlazorServer()


let app = builder.Build()

app.UseStaticFiles()
app.UseAntiforgery()

app.MapRazorComponents()
app.MapSSRDemoes()

app.Run()
