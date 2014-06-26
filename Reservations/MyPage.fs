namespace Appointments2

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.JQuery
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper.Formlet.Controls
open IntelliFactory.WebSharper.Sitelets

module Server =
    [<JavaScript>]
    type Reserv = {
        Name : string
        Email : string
        Date : string
    }

    [<JavaScript>]
    type Pages =
        | Add
        | CheckUser
        | CheckAdmin
        | DeleteAdmin
        | DeleteUser

    let Users =
        [
            "rozsika", "almafa"
        ]

    let mutable Reservations : Reserv list = []

    let mutable LoggedinUsers : (string * bool) list = []

    let IsAdmin name =
        try
            LoggedinUsers
            |> List.find (fun a -> (fst a) = name)
            |> snd
        with 
            | :? System.Collections.Generic.KeyNotFoundException -> false

    [<Rpc>]
    let Authenticate user pw =
        UserSession.LoginUser user
        let l = Users
                |> List.filter (function
                                    | fst, snd -> fst = user && snd = pw)
                |> List.length
        LoggedinUsers <- (user, l <> 0) :: LoggedinUsers                

    [<Rpc>]
    let IsReservedAdmin (r1 : Reserv) =
        Reservations
        |> List.filter (fun r2 ->
                            let d1 = System.DateTime.ParseExact(r1.Date, "yyyy/MM/dd HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                            let d2 = System.DateTime.ParseExact(r2.Date, "yyyy/MM/dd HH:mm", System.Globalization.CultureInfo.InvariantCulture)

                            let dD =
                                if d1 > d2 then
                                    d1 - d2
                                else
                                    d2 - d1
                            
                            dD.TotalHours < 1.
                       )
        |> (function
            | [] -> None
            | x :: _ -> Some x)

    [<Rpc>]
    let IsReservedUser (r1 : Reserv) =
        Reservations
        |> List.filter (fun r2 ->
                            System.Diagnostics.Debug.WriteLine(r2.Date)
                            let d1 = System.DateTime.ParseExact(r1.Date, "yyyy/MM/dd HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                            let d2 = System.DateTime.ParseExact(r2.Date, "yyyy/MM/dd HH:mm", System.Globalization.CultureInfo.InvariantCulture)

                            let dD =
                                if d1 > d2 then
                                    d1 - d2
                                else
                                    d2 - d1
                            
                            System.Diagnostics.Debug.WriteLine(string dD.TotalHours)
                            dD.TotalHours < 1.
                       )
        |> List.length
        |> (fun a -> a > 0)


    [<Rpc>]
    let AddReservation r =
        if not <| IsReservedUser r then    
            Reservations <- r :: Reservations
            true
        else
            false

    [<Rpc>]
    let DeleteReservationAdmin r1 =
        Reservations
        |> List.filter (fun r2 ->
                            let d1 = System.DateTime.ParseExact(r1.Date, "yyyy/MM/dd HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                            let d2 = System.DateTime.ParseExact(r2.Date, "yyyy/MM/dd HH:mm", System.Globalization.CultureInfo.InvariantCulture)

                            let dD =
                                if d1 > d2 then
                                    d1 - d2
                                else
                                    d2 - d1
                            
                            dD.TotalHours >= 1.
                        )
        |> (fun l -> Reservations <- l)

    [<Rpc>]
    let DeleteReservationUser r1 =
        Reservations
        |> List.filter (fun r2 ->
                            let d1 = System.DateTime.ParseExact(r1.Date, "yyyy/MM/dd HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                            let d2 = System.DateTime.ParseExact(r2.Date, "yyyy/MM/dd HH:mm", System.Globalization.CultureInfo.InvariantCulture)

                            let dD =
                                if d1 > d2 then
                                    d1 - d2
                                else
                                    d2 - d1
                            
                            dD.TotalHours >= 1. || r2.Name <> r1.Name || r2.Email <> r1.Email
                        )
        |> (fun l -> Reservations <- l)

    

[<JavaScript>]
module Auth =
    type User = { Name : string; IsAdmin : bool }

    [<Inline "window.location = $0">]
    let Redirect url = X<unit>            

    let Main url =
        let name = 
            Input ""
            |> Validator.IsNotEmpty "Don't forget your name!"
            |> Enhance.WithValidationIcon
            |> Enhance.WithTextLabel "Name"
        let pw =
            Password ""
            |> Enhance.WithValidationIcon
            |> Enhance.WithTextLabel "Password"
        let formlet = Formlet.Yield (fun name p -> Server.Authenticate name p)
                        <*> name <*> pw
        
        let fc = 
            Enhance.FormContainerConfiguration.Default 

        let f =
            formlet
            |> Enhance.WithSubmitAndResetButtons
            |> Enhance.WithCustomFormContainer fc

        Formlet.Run (fun () -> Redirect url) f                        

[<JavaScript>]
module MyPage =

    [<Inline "($0).datetimepicker({step: 15, minDate: '0'})">]
    let DateTimeInit (a) = X<unit>

    let Form n = 
        let name = 
            Input n 
            |> Validator.IsNotEmpty "Don't forget your name!"
            |> Enhance.WithValidationIcon
            |> Enhance.WithTextLabel "Name"
        let email =
            Input "example@example.com"
            |> Validator.IsEmail "Don't forget your email address!"
            |> Enhance.WithValidationIcon
            |> Enhance.WithTextLabel "E-mail"
        let date =
            Input ""
            |> Enhance.WithTextLabel "Date"
            |> Enhance.WithCssClass "datetime"
        Formlet.Yield (fun n a d -> { Server.Reserv.Name = n; Server.Reserv.Email = a; Server.Reserv.Date = d })
        <*> name
        <*> email
        <*> date

    let select (page : Server.Pages) (r : Server.Reserv) =
        match page with 
            | Server.Pages.Add -> if Server.AddReservation(r) then 
                                    JavaScript.Alert("Success") else
                                    JavaScript.Alert("Already reserved")
            | Server.Pages.CheckAdmin -> match Server.IsReservedAdmin(r) with 
                                            | None -> JavaScript.Alert("Available")
                                            | Some x -> JavaScript.Alert("Name: " + x.Name + "\nEmail: " + x.Email)
            | Server.Pages.CheckUser -> if Server.IsReservedUser(r) then
                                            JavaScript.Alert("Reserved") else
                                            JavaScript.Alert("Available")
            | Server.Pages.DeleteAdmin -> Server.DeleteReservationAdmin(r)
            | Server.Pages.DeleteUser -> Server.DeleteReservationUser(r)

    let Main (name : string) (page : Server.Pages) = 
        JavaScript.Log name

        let fc = {
            Enhance.FormContainerConfiguration.Default with
                Header = "Hello " + string name |> Enhance.FormPart.Text |> Some
                Description =
                    "Fill in the form"
                    |> Enhance.FormPart.Text
                    |> Some
            }
        let f =
            Form name
            |> Enhance.WithSubmitAndResetButtons
            |> Enhance.WithCustomFormContainer fc

        Formlet.Run (select page) f

    let InitDateTime () =
        DateTimeInit <| JQuery.Of(".datetime").Find("input")
        Div [] :> IPagelet