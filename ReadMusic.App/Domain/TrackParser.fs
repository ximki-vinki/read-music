module ReadMusic.App.Domain.TrackParser

open System.IO
open TagLib
open Serilog

let parse (path: string) : Track option =
    try
        use file = File.Create path
        let tag = file.Tag

        let track = {
            Number = Some (string tag.Track)
            Path = path
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
        //TODO сделать потом debug
        Log.Information("Успешный парсинг: {Path}", path)
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