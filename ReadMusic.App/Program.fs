module Program

open ReadMusic.App.Domain.TrackParser
open ReadMusic.App.Infrastructure.FileSystem
open System.Diagnostics
open System.IO

[<EntryPoint>]
let main _ =
    let rootPath = "/media/ximki-vinki/C487EA472D10D620/music/Мельница"

    let imageExts = set [".jpg"; ".jpeg"; ".png"; ".gif"; ".bmp"; ".tiff"; ".webp"]

    let sw = Stopwatch.StartNew()
    let results = ResizeArray<string * int64>() // (path, duration_ticks)
    let mutable count = 0

    scanDirectoryRecursively rootPath
    |> Seq.sortBy Path.GetFileName
    |> Seq.iter (fun path ->
        if imageExts.Contains(Path.GetExtension(path).ToLowerInvariant()) then
            // Пропускаем изображения
            ()
        else
            count <- count + 1
            printf "\r🔄 Обрабатываю %d: %s" count (Path.GetFileName path)

            let t0 = Stopwatch.GetTimestamp()
            ignore (parse path)
            let t1 = Stopwatch.GetTimestamp()
            let dt = t1 - t0

            results.Add(path, dt)
    )

    sw.Stop()

    printfn "\n\n✅ Обработка завершена. Всего файлов: %d" results.Count
    printfn "⏱  Общее время: %A\n" sw.Elapsed

    // Топ-20 самых медленных
    results
    |> Seq.sortByDescending snd
    |> Seq.truncate 20
    |> Seq.iteri (fun i (path, dt) ->
        let ms = (float dt) * 1000.0 / float Stopwatch.Frequency
        printfn "%2d. [%.1f ms] %s" (i + 1) ms path
    )

    0