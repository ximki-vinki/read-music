module ReadMusic.App.Domain.Meta

open ReadMusic.App.Domain
open TagLib

let detect (path: string) : AudioFormat =
    try
        use file = File.Create path
        match file with
        | :? Flac.File -> Flac
        | :? Mpeg.File -> Mp3
        | :? Ogg.File  -> Ogg
        | _ -> Unknown
    with _ -> Unknown