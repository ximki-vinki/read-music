module Program

open ReadMusic.App.Domain.TrackParser
open ReadMusic.App.Infrastructure.FileSystem
open System.IO

[<EntryPoint>]
let main _ =
    ReadMusic.App.Infrastructure.Database.ensureSchema()

    let rootPath = "/media/ximki-vinki/C487EA472D10D620/music/Volturian/"
    let musicExts =
        set [ ".mp3"; ".flac"; ".m4a"; ".wav"; ".ogg"; ".opus"; ".wv"; ".ape"; ".aiff" ]

    scanDirectoryRecursively rootPath
    |> Seq.filter (fun path ->
        let ext = Path.GetExtension(path).ToLowerInvariant()
        musicExts.Contains ext)
    |> Seq.sortBy Path.GetFileName
    |> Seq.iter (fun path -> ignore (parse path))
    
    0