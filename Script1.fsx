#r @"packages\Suave.0.30.0\lib\net40\Suave.dll"

open Suave                 // always open suave
open Suave.Http.Successful // for OK-result
open Suave.Web             // for config

startWebServer defaultConfig (OK "Hello World!")


#r @"packages\FSharp.Configuration.0.5.3\lib\net40\FSharp.Configuration.dll"
#r @"C:\wd\OAuth2InstagramClient\packages\FSharp.Configuration.0.5.3\lib\net40\FSharp.Configuration.dll"

open FSharp.Configuration

type Settings = AppSettings<"App1.config"> // doh! bug here - this isn't working
printfn "%A" Settings.ConfigFileName
printfn "%A" Settings.TestInt

open System.IO

let [|oauthClientId; oauthSecret|] =  File.ReadAllLines(__SOURCE_DIRECTORY__ + "\\oauth.secret")


