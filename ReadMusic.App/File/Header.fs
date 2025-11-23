module ReadMusic.App.File.Header

open System.IO
open ReadMusic.App.Domain.Meta

let readHeader (path: string) : Result<byte[], string> =
    try
        use stream = File.OpenRead path
        let buffer = Array.zeroCreate requiredHeaderSize
        let read = stream.Read(buffer, 0, buffer.Length)
        Ok (Array.sub buffer 0 read)
    with
    | :? FileNotFoundException -> Error $"File not found: {path}"
    | :? IOException as e -> Error $"IO error: {e.Message}"
    | ex -> Error $"Unexpected: {ex.Message}"