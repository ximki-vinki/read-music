module Program

open ReadMusic.App.Domain.TrackParser
open ReadMusic.App.Infrastructure.FileSystem
open System.IO

[<EntryPoint>]
let main _ =
    let rootPath = "/media/ximki-vinki/C487EA472D10D620/music/Era/1996 - Era"

    scanDirectoryRecursively rootPath
    |> Seq.sortBy Path.GetFileName
    |> Seq.iter (fun path ->
        printfn "──────────────────────────"
        match parse path with
        | None ->
            eprintfn $"Failed to read: {path}"
        | Some track ->
            printfn $"File: {track.Path}"
            printfn $"Container: {track.Container}"
            printfn $"Extension: {track.Extension}"
            printfn $"Tag types: {track.TagTypes}"
            track.Number |> Option.iter (printfn "Track #: %s")
            track.Metadata.Title  |> Option.iter (printfn "Title: %s")
            track.Metadata.Artist |> Option.iter (printfn "Artist: %s")
            track.Metadata.Album  |> Option.iter (printfn "Album: %s")
            track.Metadata.Year   |> Option.iter (printfn "Year: %s")
    )
    0