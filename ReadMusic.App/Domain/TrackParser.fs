module ReadMusic.App.Domain.TrackParser

open System.IO
open TagLib
open Serilog

let parse (path: string) : Track option =
    try
        use file = File.Create path
        let tag = file.Tag
        let fileInfo = FileInfo(path)
        
        let track = {
            Number = Some (string tag.Track)
            Path = path
            FileSize = fileInfo.Length
            FileModifiedAt = fileInfo.LastWriteTimeUtc.ToString("yyyy-MM-ddTHH:mm:ssZ")
            Container = file.MimeType.Split('/')[1]
            TagTypes = tag.TagTypes
            Extension = Path.GetExtension path |> _.ToLowerInvariant()
            Metadata = {
                Title  = Option.ofObj tag.Title
                Artist = Option.ofObj tag.FirstPerformer
                Album  = Option.ofObj tag.Album
                Year   = Some (string tag.Year)
            }
        }
        Log.Debug("Успешный парсинг: {Path}", path)
        Some track
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