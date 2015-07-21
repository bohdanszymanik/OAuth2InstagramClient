// todo: use the project scaffold to set up correctly on github https://github.com/fsprojects/ProjectScaffold

open System
open Suave
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Http.RequestErrors
open Suave.Web
open Suave.Types
open System.Threading

(* 
    Read secret configuration
    Assumes the existence of secret.config (to be excluded with .gitignore of course) with a structure like this:
    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
        <appSettings>
            <add key="oauthClientId" value="some client id"/>
            <add key="oauthSecret" value="some secret"/>
        </appSettings>
    </configuration>

except I can't get FSharp.Configuration to work in this project
*)
//open FSharp.Configuration

//type Settings = AppSettings<"secret.config"> // doh! bug here - this isn't working
// so fall back onto something else
open System.IO
let [|clientId; secret|] =  File.ReadAllLines(__SOURCE_DIRECTORY__ + "\\oauth.secret")

let redirectUri = "http://localhost:1410"

// define the function here to retrieve the json encoded instagram data
let tagsMediaRecentUrl tagName accessToken =
    sprintf "https://api.instagram.com/v1/tags/%s/media/recent?access_token=%s" tagName accessToken

let serverConfig =
  { defaultConfig with
      homeFolder = Some __SOURCE_DIRECTORY__
      logger = Logging.Loggers.saneDefaultsFor Logging.LogLevel.Debug
      bindings = [ HttpBinding.mk' HTTP  "127.0.0.1" 1410] }

// this is a representative redirect url from instagram
// http://localhost:1410/?code=9ab66fabd418419e8ea9e73123127c0f&state=ewpBNSn7W7
open FSharp.Data
let postCode code =
    Http.RequestString("https://api.instagram.com/oauth/access_token", 
//                        body=FormValues [   "client_id",Settings.OaUthClientId;
//                                            "client_secret", Settings.OaUthSecret;
                        body=FormValues [   "client_id",clientId;
                                            "client_secret", secret;
                                            "grant_type", "authorization_code";
                                            "redirect_uri", redirectUri;
                                            "code", code])

type tokenJson = JsonProvider<"""{
    "access_token": "fb2e77d.47a0479900504cb3ab4a1f626d174d2d",
    "user": {
        "id": "1574083",
        "username": "snoopdogg",
        "full_name": "Snoop Dogg",
        "profile_picture": "..."
    }
}"""> // as copied from Instagram docs https://instagram.com/developer/authentication/

let accessToken =
    fun json ->
    let tokenParser = tokenJson.Parse json
    tokenParser.AccessToken

let handleRedirect =
    request ( fun r ->
        match r.queryParam "code" with
        | Choice1Of2 code -> OK (code |> postCode |> accessToken)
//        | Choice1Of2 code -> OK ( (postCode >> accessToken) code) //same same but different
        | Choice2Of2 msg -> BAD_REQUEST msg)

    //sprintf "Here's the url from Instagram : %s now we can execute 
    //                    a function to retrieve the json encoded data for our query" req.rawQuery
    


let app : WebPart =
  choose
    [ path "/" >>= handleRedirect ]



[<EntryPoint>]
let main argv = 

    let cts = new CancellationTokenSource()
    let startingServer, shutdownServer = startWebServerAsync serverConfig app
 
    Async.Start(shutdownServer, cts.Token)
 
    startingServer |> Async.RunSynchronously |> printfn "started: %A"
 
    // need to startup a browser session, point it to instagram with the key and secret etc
    // then it will be redirected to localhost:1410 where we need to handle it and move on

    let url = sprintf "https://api.instagram.com/oauth/authorize/?client_id=%s&redirect_uri=%s&response_type=code" clientId redirectUri
    System.Diagnostics.Process.Start("iexplore.exe", url) |> ignore

    printfn "Press Enter to stop"
    Console.Read() |> ignore
 
    cts.Cancel()
 
    0
