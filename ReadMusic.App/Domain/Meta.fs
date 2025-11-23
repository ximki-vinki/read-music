module ReadMusic.App.Domain.Meta

let requiredHeaderSize = 16

module private Bytes =
    let startsWith (prefix: byte[]) (data: byte[]) : bool =
        prefix.Length <= data.Length &&
        Array.forall2 (=) prefix (data.[.. prefix.Length - 1])

    let B (s: string) : byte[] = System.Text.Encoding.ASCII.GetBytes s

let getSignature = function
    | Mp3   -> Bytes.B "ID3"
    | Flac  -> Bytes.B "fLaC"
    | Ogg   -> Bytes.B "OggS"
    | Unknown -> [||]

let private supportedFormats = [ Mp3; Flac; Ogg ]

let detect (bytes: byte[]) : AudioFormat =
    supportedFormats
    |> List.tryFind (fun fmt -> Bytes.startsWith (getSignature fmt) bytes)
    |> Option.defaultValue Unknown