module Program

open ReadMusic.App.Domain.TrackParser
open ReadMusic.App.Infrastructure.FileSystem
open System.Diagnostics
open System.IO

[<EntryPoint>]
let main _ =
    let rootPath = "/media/ximki-vinki/C487EA472D10D620/music/Era"

    let sw = System.Diagnostics.Stopwatch.StartNew()
    let mutable count = 0

    scanDirectoryRecursively rootPath
    |> Seq.sortBy System.IO.Path.GetFileName
    |> Seq.iter (fun path ->
        count <- count + 1
        printf "\rПрочитано %d файлов..." count
        ignore (parse path)  // просто вызываем, результат не используем
    )

    sw.Stop()
    printfn "\n\n✅ Всего обработано файлов: %d" count
    printfn "⏱  Время выполнения: %A" sw.Elapsed

    0