namespace Appointments2

open IntelliFactory.Html
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Sitelets

type Action =
    | Login
    | MainSite of Server.Pages
    | Menu

module Controls =

    [<Sealed>]
    type EntryPoint (name, num) =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            MyPage.Main name num

    [<Sealed>]
    type Hekk() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            MyPage.InitDateTime ()

    [<Sealed>]
    type LoginEntry (url) =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            Auth.Main url

module Skin =
    open System.Web

    type Page =
        {
            Title : string
            Body : list<Content.HtmlElement>
        }

    let MainTemplate =
        Content.Template<Page>("~/Main.html")
            .With("title", fun x -> x.Title)
            .With("body", fun x -> x.Body)

    let WithTemplate title body : Content<Action> =
        Content.WithTemplate MainTemplate <| fun context ->
            {
                Title = title
                Body = body context
            }

module Site =

    let ( => ) text url =
        A [HRef url] -< [Text text]

    let Links (ctx: Context<Action>) =
        let basel =
            [
                LI ["Add reservation" => ctx.Link (Action.MainSite Server.Pages.Add)]                
            ]
        match UserSession.GetLoggedInUser () with
            | Some name ->
                if Server.IsAdmin name then
                    UL (LI ["Delete reservation" => ctx.Link (Action.MainSite Server.Pages.DeleteAdmin)] :: 
                        LI ["Check reservation" => ctx.Link (Action.MainSite Server.Pages.CheckAdmin)] ::
                        basel)
                else
                    UL (LI ["Delete reservation" => ctx.Link (Action.MainSite Server.Pages.DeleteUser)] :: 
                        LI ["Check reservation" => ctx.Link (Action.MainSite Server.Pages.CheckUser)] ::
                        basel)
            | None -> UL basel

    let MainSiteGen num =
        Skin.WithTemplate "Stuff" <| fun ctx ->
                let name = 
                        UserSession.GetLoggedInUser ()
                        |> Option.get
                [
                    Div [new Controls.EntryPoint(name, num)]
                    Div [new Controls.Hekk()]
                ]            

    let MenuPage = 
        Skin.WithTemplate "Menu" <| fun ctx ->
            [
                Links ctx
            ]

    let LoginPage =
        Skin.WithTemplate "Login" <| fun ctx ->
            [
                Div [new Controls.LoginEntry (ctx.Link Action.Menu)]
            ]

    let Main =       
        let a = Sitelet.Content "/" Login LoginPage

        let PageGen num =
            let filter: Sitelet.Filter<Action> =
                {
                    VerifyUser = fun _ -> true
                    LoginRedirect = fun _ -> Action.Login
                }
            Sitelet.Protect filter <| Sitelet.Content ("/mainsite" + string(num.GetHashCode())) (Action.MainSite num) (MainSiteGen num)

        let menu = 
            let filter: Sitelet.Filter<Action> =
                {
                    VerifyUser = fun _ -> true
                    LoginRedirect = fun _ -> Action.Login
                }
            Sitelet.Protect filter <| Sitelet.Content "/menu" Action.Menu MenuPage

        Sitelet.Sum [
            a
            PageGen Server.Pages.Add
            PageGen Server.Pages.CheckAdmin
            PageGen Server.Pages.CheckUser
            PageGen Server.Pages.DeleteAdmin
            PageGen Server.Pages.DeleteUser
            menu
        ]


[<Sealed>]
type Website() =
    interface IWebsite<Action> with
        member this.Sitelet = Site.Main
        member this.Actions = [MainSite Server.Pages.Add; MainSite Server.Pages.CheckAdmin; MainSite Server.Pages.CheckUser;
                                MainSite Server.Pages.DeleteAdmin; MainSite Server.Pages.DeleteUser; Login; Menu]

type Global() =
    inherit System.Web.HttpApplication()

    member g.Application_Start(sender: obj, args: System.EventArgs) =
        ()

[<assembly: Website(typeof<Website>)>]
do ()
