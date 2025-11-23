module Program

open ReadMusic.App.Domain.TrackParser
open ReadMusic.App.Infrastructure.FileSystem
open System.Diagnostics
open System.IO

[<EntryPoint>]
let main _ =
    let rootPath = "/media/ximki-vinki/C487EA472D10D620/music/Era"

    let mutable totalTime = 0L
    let mutable ioTime = 0L
    let mutable parseTime = 0L

    scanDirectoryRecursively rootPath
    |> Seq.sortBy Path.GetFileName
    |> Seq.iter (fun path ->
        let t0 = Stopwatch.GetTimestamp()

        let stream, t1 =
            let t = Stopwatch.GetTimestamp()
            let s = File.OpenRead path
            s, Stopwatch.GetTimestamp()

        let res, t2 =
            let t = Stopwatch.GetTimestamp()
            let r = parse path
            r, Stopwatch.GetTimestamp()

        stream.Dispose()  // важно — если parse не забирает stream

        let dt_io = t1 - t0
        let dt_parse = t2 - t1
        let dt_total = t2 - t0

        ioTime <- ioTime + dt_io
        parseTime <- parseTime + dt_parse
        totalTime <- totalTime + dt_total

        let ms_io = float dt_io * 1000.0 / float Stopwatch.Frequency
        let ms_parse = float dt_parse * 1000.0 / float Stopwatch.Frequency
        printfn "%s → I/O: %.1f мс, Parse: %.1f мс" (Path.GetFileName path) ms_io ms_parse
    )

    let ms_total = float totalTime * 1000.0 / float Stopwatch.Frequency
    let ms_io = float ioTime * 1000.0 / float Stopwatch.Frequency
    let ms_parse = float parseTime * 1000.0 / float Stopwatch.Frequency

    printfn "\n📊 Итого: %.1f мс" ms_total
    printfn "   I/O: %.1f мс (%.1f%%)" ms_io (100.0 * ms_io / ms_total)
    printfn "   Parse: %.1f мс (%.1f%%)" ms_parse (100.0 * ms_parse / ms_total)

    0