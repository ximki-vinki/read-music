module ReadMusic.App.Domain.TrackParser

open System.IO
open TagLib

let parse (path: string) : Track option =
    try
        use file = File.Create path
        let tag = file.Tag

        Some {
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
    with ex ->
        eprintfn $"Error reading '{path}': {ex.Message}"
        None