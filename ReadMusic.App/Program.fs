open ReadMusic.App

[<EntryPoint>]
let main _ =
    let path = "/media/ximki-vinki/C487EA472D10D620/music/Era/1996 - Era/01 - Era.flac"
    match File.tryReadBytes path with
    | Ok bytes -> 
        match File.detectFormat bytes with
        | File.Mp3   -> printfn "MP3"
        | File.Flac  -> printfn "FLAC"
        | File.Unknown -> printfn "???"
    | Error (File.FileNotFound p) -> printfn "Not found: %s" p
    | Error (File.IOError m)      -> printfn "IO: %s" m
    0