module Instagrams.View

open Suave.Html

let divId id = divAttr ["id", id]
let h1 xml = tag "h1" [] xml
let aHref href = tag "a" ["href", href]
let form x = tag "form" ["method", "POST"] (flatten x)
let submitInput value = inputAttr ["type", "submit"; "value", value]

let instagramQueries = 
    html [
        head [
            title "Instagram Queries"
        ]

        body [
            divId "header" [
                h1 (aHref "/" (text "Instagram Queries"))
            ]

            divId "main" [
                p [
                    text "Idea here is to have a form with box to put in the list of tags then a button to POST them back to be used in the recent media query on the instagram api"
                ]
                form [
                    submitInput "Submit tags"
                ]
            ]

            divId "footer" [
                text "built with "
                aHref "http://fsharp.org" (text "F#")
                text " and "
                aHref "http://suave.io" (text "Suave.IO")
            ]
        ]
    ]
    |> xmlToString
