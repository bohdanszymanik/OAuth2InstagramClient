#r @"C:\wd\OAuth2InstagramClient\packages\FSharp.Configuration.0.5.3\lib\net40\FSharp.Configuration.dll"
open System
open FSharp.Configuration
type settings = AppSettings<"App.config">

printfn "%s" settings.ConfigFileName
printfn "%s" settings.Test
printfn "%s" settings.ClientId
printfn "%s" settings.Secret

Console.ReadLine()

let negate x = x * -1 
let square x = x * x 
let print  x = printfn "The number is: %d" x
let square_negate_then_print = square >> negate >> print 
square_negate_then_print 2



(*
    sequence of possible SAP error codes
    format
        3 numeric characters
        (
        20 characters that we believe can be upper case or / or _
        )
    giving a total of 25 characters
*)
 
open System

let rndCode =
    let r = System.Random(Guid.NewGuid().GetHashCode())
    let validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ()_".ToCharArray()
    fun() -> // embed the call to r in a closure to get it to stay 'alive' with seed reseeding...
        [|for _ in 1..25 -> validChars.[r.Next(29)] |]
 
let codeSamples =
    Seq.init 100 (fun _ -> rndCode() )
    |> Seq.map (fun sample ->
                        sample |> Array.fold (fun acc sample -> acc + (sample.GetHashCode() * 101) % 0x40000000) 0
                )
    |> Seq.toArray // mutable structure stops it recalculating

// just to see what the strings look like
//Seq.init 100 (fun _ -> [|for _ in 1..25 -> validChars.[r.Next(29)] |]) |> Seq.map (fun strArray -> strArray |> Array.toSeq |> String.Concat)
//|> Seq.iter ( printfn "%s" )

codeSamples |> Seq.distinct |> Seq.length
 
codeSamples |> Array.distinct |> Array.length
codeSamples |> Seq.iter (printfn "%d" )

Seq.init 10000 (fun _ -> rndCode() )
|> Seq.distinct
|> Seq.length




#r @".\packages\FSharp.Data.2.2.5\lib\net40\FSharp.Data.dll"
open FSharp.Data
type SAPErrors = CsvProvider<Sample="c:\\temp\\SAPErrors.txt", Separators="|", HasHeaders=true>
let sapErrors = new SAPErrors()

(sapErrors.Rows |> Seq.head).ErrorNumber

sapErrors.Rows
|> Seq.take 10
|> Seq.iter (fun r -> printfn "%03i(%s)" r.ErrorNumber (r.ErrorType.Trim()))

open System.Runtime.Remoting.Metadata.W3cXsd2001
SoapHexBinary.Parse("11cabebf97194f36898e1c0544c56752").Value


