module ReadMusic.App.Domain.TrackReader

open System.IO
open TagLib
open Serilog

let readFileData (path: string) : FileData =
    let fileInfo = FileInfo(path)
    {
        Path = path
        FileSize = fileInfo.Length
        FileModifiedAt = fileInfo.LastWriteTimeUtc.ToString("yyyy-MM-ddTHH:mm:ssZ")
        Extension = Path.GetExtension path |> _.ToLowerInvariant()
    }

let parseMetadata (path: string) : TrackMetadata option =
    try
        use file = File.Create path
        let tag = file.Tag
        let metadata = {
            Title = Option.ofObj tag.Title
            Artist = Option.ofObj tag.FirstPerformer
            Album = Option.ofObj tag.Album
            Year = Some (string tag.Year)
            Number = Some (string tag.Track)
            TagTypes = tag.TagTypes
            Container = file.MimeType.Split('/')[1]
        }
        Log.Debug("Успешный парсинг метаданных: {Path}", path)
        Some metadata
    with
    | :? CorruptFileException as ex ->
        Log.Warning(ex, "Пропущен {Path}: битый файл", path)
        None
    | :? UnsupportedFormatException as ex ->
        Log.Warning(ex, "Пропущен {Path}: неподдерживаемый формат", path)
        None
    | ex ->
        Log.Error(ex, "Неизвестная ошибка парсинга {Path}", path)
        None