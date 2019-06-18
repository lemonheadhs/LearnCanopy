#load ".paket/load/Selenium.WebDriver.fsx"
#load ".paket/load/main.group.fsx"

open System
open canopy.runner.classic
open canopy.configuration
open OpenQA.Selenium
open canopy.classic

// canopy.configuration.chromeDir <- System.AppContext.BaseDirectory
canopy.configuration.chromeDir <- """C:\Users\admin\.nuget\packages\selenium.webdriver.chromedriver\74.0.3729.6\driver\win32"""
compareTimeout <- 40.


//start an instance of chrome
start chrome

//go to url
url "https://mytestsite.net/umbraco"
waitFor <| fadedIn "#login input[name=username]"

"#login input[name=username]" << ""
"#login input[name=password]" << ""
click "#login button.btn[type=submit]"
waitFor <| fadedIn "img#avatar-img"

url "https://mytestsite.net/umbraco/#/content/content/edit/13539"
sleep 4
waitFor <| fadedIn "#contentcolumn ul.umb-nav-tabs"

let propTab = elementWithText "#contentcolumn ul.umb-nav-tabs a[data-toggle=tab]" "Properties"
click propTab

let tabBodyId = propTab.GetAttribute("href") |> Uri |> fun u -> u.Fragment
let templateSelect =
    let parentEle = 
        tabBodyId 
        |> sprintf "#contentcolumn div%s label[for=_umb_template]" 
        |> element 
        |> parent
    elementWithin "select[name=dropDownList]" parentEle
let theOption =
    elementWithin "option[value=NewDetail20190617]" templateSelect

click templateSelect
click theOption

let saveBtn =
    let anchorText = sprintf "#contentcolumn div%s div.umb-bottom-bar > div.dropup > a > localize[key=buttons_saveAndPublish]" tabBodyId
    element anchorText |> parent

click saveBtn

sleep 15
waitFor <| fadedIn "#speechbubble > ul > li.alert-success"


quit()