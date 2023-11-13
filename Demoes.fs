[<AutoOpen>]
module Demos

open System
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Fun.Blazor
open Fun.Htmx

let layout (node: NodeRenderFragment) = html' {
    head {
        baseUrl "/"
        link {
            rel "icon"
            type' "image/png"
            href "favicon.png"
        }
    }
    body {
        node
        script { src "_framework/blazor.web.js" }
        script { src "https://unpkg.com/htmx.org@1.9.8" }
    }
}

let demoItems =
    html.inject (fun (hook: IComponentHook) ->
        let mutable items = []

        hook.AddInitializedTask(fun () -> task {
            do! Task.Delay 2000
            items <- [ 1..5 ]
        })

        div {
            if items.Length = 0 then
                progress.create ()
            else
                ul {
                    for item in items do
                        li {
                            style { color "green" }
                            $"item - {item}"
                        }
                }
        }
    )

let htmxDemoView1 = div { h3 { "htmx view" } }

let ssrPage1 =
    layout (
        main {
            h1 { "Fun Blazor" }

            h2 { "Blazor Streaming" }
            html.streaming demoItems

            h2 { "Htmx demo (will start loading in 2 seconds)" }
            div {
                hxGet "/ssr/htmx-view"
                hxTrigger' (hxEvt.load, delayMs = 2000)
            }
        }
    )


type WebApplication with

    member app.MapSSRDemoes() =
        let ssr = app.MapGroup("/ssr").AddFunBlazor()
        ssr.MapGet("/demo1", Func<_>(fun () -> ssrPage1)) |> ignore
        ssr.MapGet("/htmx-view", Func<_>(fun () -> htmxDemoView1)) |> ignore
