module ReadMusic.App.Domain.TrackParser

open System.IO
open System.Diagnostics


let parse (path: string) : Track option =
    let sw = Stopwatch.StartNew()

    try
        let step1 = sw.ElapsedMilliseconds
        use file = TagLib.File.Create path
        let step2 = sw.ElapsedMilliseconds
        let tag = file.Tag
        let step3 = sw.ElapsedMilliseconds

        let result = Some {
            Number = if tag.Track > 0u then Some (string tag.Track) else None
            Path = path
            Container = file.MimeType.Split('/')[1]
            TagTypes = tag.TagTypes
            Extension = Path.GetExtension path |> (fun s -> s.ToLowerInvariant())
            Metadata = {
                Title  = Option.ofObj tag.Title
                Artist = Option.ofObj tag.FirstPerformer
                Album  = Option.ofObj tag.Album
                Year   = if tag.Year > 0u then Some (string tag.Year) else None
            }
        }

        let step4 = sw.ElapsedMilliseconds

        if step4 > 10 then  // если >10 мс — логгируем
            printfn "[SLOW] %s: Open=%d ms, Tag=%d ms, Build=%d ms" 
                (Path.GetFileName path) (step2 - step1) (step3 - step2) (step4 - step3)

        result

    with ex ->
        eprintfn "[ERROR] %s: %s" path ex.Message
        None