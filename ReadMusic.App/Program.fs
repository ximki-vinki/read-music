module Program

open Serilog
open ReadMusic.App.Domain.TrackParser
open ReadMusic.App.Infrastructure.FileSystem
open ReadMusic.App.Infrastructure.Database
open System.IO

[<EntryPoint>]
let main _ =
    Log.Logger <- LoggerConfiguration().WriteTo.Console().CreateLogger()

    Log.Information("Запуск программы")

    ensureSchema ()

    let rootPath = "/media/ximki-vinki/C487EA472D10D620/music/Volturian/"

    let musicExts =
        set
            [ ".mp3"
              ".flac"
              ".m4a"
              ".wav"
              ".ogg"
              ".opus"
              ".wv"
              ".ape"
              ".aiff" ]

    Log.Information(
        "Сканирование пути: {RootPath}, поддерживаемые расширения: {MusicExts}",
        rootPath,
        List.ofSeq musicExts
    )

    scanDirectoryRecursively rootPath
    |> Seq.filter (fun path ->
        let ext = Path.GetExtension(path).ToLowerInvariant()
        musicExts.Contains ext)
    |> Seq.sortBy Path.GetFileName
    |> Seq.iter (fun path ->
        match parse path with
        | Some track -> insertTrack track
        | None -> ())

    Log.CloseAndFlush()
    0
