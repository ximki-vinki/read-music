module Program

open System.IO
open ReadMusic.App.File.Header
open ReadMusic.App.Domain.Meta

[<EntryPoint>]
let main _ =
    let path = "/media/ximki-vinki/C487EA472D10D620/music/Era/1996 - Era/01 - Era.flac"

    match readHeader path with
    | Error msg ->
        eprintfn $"Error %s{msg}"
        1
    | Ok header ->
        let format = detect header
        printfn $"%s{Path.GetFileName path}: %A{format}"
        0