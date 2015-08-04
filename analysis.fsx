
#r @"packages\FSharp.Data.2.2.5\lib\net40\FSharp.Data.dll"
open FSharp.Data
open System.IO

type Stitch = JsonProvider<"c:\\temp\\stitching.json">
let stitch = Stitch.Parse(File.ReadAllText("c:\\temp\\stitching.json"))

stitch.Data
|> Seq.iter (fun i -> 
                    printfn "%A, %A, %A" i.CreatedTime i.Location i.Tags )
