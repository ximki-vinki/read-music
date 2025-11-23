module ReadMusic.App.Infrastructure.FileSystem

open System.IO

let private safeGetFiles path =
    try Directory.GetFiles(path) |> Array.toSeq
    with _ -> Seq.empty

let private safeGetDirectories path =
    try Directory.GetDirectories(path) |> Array.toSeq
    with _ -> Seq.empty

let scanDirectoryRecursively (root: string) : seq<string> =
    let rec loop currentPath =
        seq {
            yield! safeGetFiles currentPath
            for dir in safeGetDirectories currentPath do
                yield! loop dir
        }
    loop root
