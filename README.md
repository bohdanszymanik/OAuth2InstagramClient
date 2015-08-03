# OAuth2InstagramClient
F# implementation of an oauth 2.0 server side (explicit) authentication flow using http://Suave.IO and 
the http://fsharp.github.io/FSharp.Data/library/Http.html package to construct the required HTTPS POST.

It's part of an analysis task being undertaken by someone (my wife :) ) who needs to get instagram data for recent media that conform to certain tags (that happen to be associated with craft activities). The json formattted response just gets written to disk and as a secondary step we can read/query the data interactively with the FSharp.Data JSON type provider - check Analysis.fsx. The idea is simply to pull out dates, locations, the other tags associated with the instagram, comments for lexical analysis etc.
