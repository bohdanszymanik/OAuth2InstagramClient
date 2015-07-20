#r @"packages\Suave.0.30.0\lib\net40\Suave.dll"

open System
open Suave
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Web
open Suave.Types

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

let app : WebPart =
  choose
    [ path "/" >>= request (fun req -> OK (sprintf "Here's the url from Instagram : %s 
                                                    now we can execute a function to retrieve 
                                                    the json encoded data for our query" req.rawQuery )) ]

startWebServer serverConfig app

// need to startup a browser session, point it to instagram with the key and secret etc
// then it will be redirected to localhost:1410 where we need to handle it and move on

let url = sprintf "https://api.instagram.com/oauth/authorize/?client_id=%s&redirect_uri=%s&response_type=code" clientId redirectUri

System.Diagnostics.Process.Start("iexplore.exe", url)
