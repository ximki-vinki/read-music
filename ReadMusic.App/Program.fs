module Program

open ReadMusic.App.Infrastructure.Counter
open Serilog
open ReadMusic.App.Domain.TrackReader
open ReadMusic.App.Domain.ImportStats
open ReadMusic.App.Infrastructure.FileSystem
open ReadMusic.App.Infrastructure.Database
open System.IO
open ReadMusic.App.Domain

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

    let updateLine (stats: ImportStats) =
        System.Console.Write($"\r успешно: {stats.Success.Value} | пропущено {stats.Skipped.Value}")

    let processFile stats path =
        let fileData = readFileData path

        match parseMetadata path with
        | Some metadata ->
            let track = { FileData = fileData; Metadata = metadata }
            insertTrack track
            let updated = ImportStats.incrementSuccess stats
            updateLine updated
            updated
        | None ->
            let updated = ImportStats.incrementSkipped stats
            updateLine updated
            updated

    let finalStats = 
        scanDirectoryRecursively rootPath
        |> Seq.filter (fun path ->
            let ext = Path.GetExtension(path).ToLowerInvariant()
            musicExts.Contains ext)
        |> Seq.sortBy Path.GetFileName
        |> Seq.fold processFile ImportStats.zero

    System.Console.WriteLine()

    Log.Information(
        "Итог: успешно {Success}, пропущено {Skipped}",
        finalStats.Success.Value,
        finalStats.Skipped.Value
    )

    Log.CloseAndFlush()
    0
