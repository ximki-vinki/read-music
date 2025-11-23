module Program

open System.IO
open ReadMusic.App.Domain.Meta

[<EntryPoint>]
let main _ =
    let path = "/media/ximki-vinki/C487EA472D10D620/music/Era/1996 - Era/01 - Era.flac"

    let format = detect path
    printfn $"%s{Path.GetFileName path}: %A{format}"
    0