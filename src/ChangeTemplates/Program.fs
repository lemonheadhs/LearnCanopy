// Learn more about F# at http://fsharp.org

open System
open canopy.configuration
open OpenQA.Selenium
open canopy.parallell.functions

canopy.configuration.chromeDir <- System.AppContext.BaseDirectory
compareTimeout <- 40.

let swap f x y = f y x

let run list =
    let browser = start chrome

    try
        //go to url
        url "https://mytestsite.net/umbraco" browser
        waitFor <| fadedIn "#login input[name=username]" browser

        write "#login input[name=username]" "" browser
        write "#login input[name=password]" "" browser
        click "#login button.btn[type=submit]" browser
        waitFor <| fadedIn "img#avatar-img" browser

        url "https://mytestsite.net/umbraco/#/content/content/edit/13539" browser
        sleep 4
        waitFor <| fadedIn "#contentcolumn ul.umb-nav-tabs" browser

        let propTab = elementWithText "#contentcolumn ul.umb-nav-tabs a[data-toggle=tab]" "Properties" browser
        click propTab browser

        let tabBodyId = propTab.GetAttribute("href") |> Uri |> fun u -> u.Fragment
        let templateSelect =
            let parentEle = 
                tabBodyId 
                |> sprintf "#contentcolumn div%s label[for=_umb_template]" 
                |> swap element browser 
                |> swap parent browser
            elementWithin "select[name=dropDownList]" parentEle browser
        let theOption =
            elementWithin "option[value=NewDetail20190617]" templateSelect browser

        click templateSelect browser
        click theOption browser

        let saveBtn =
            let anchorText = sprintf "#contentcolumn div%s div.umb-bottom-bar > div.dropup > a > localize[key=buttons_saveAndPublish]" tabBodyId
            anchorText |> swap element browser |> swap parent browser

        click saveBtn browser

        sleep 15
        waitFor <| fadedIn "#speechbubble > ul > li.alert-success" browser
    finally
        quit browser

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    //start an instance of chrome
        


    0 // return an integer exit code
