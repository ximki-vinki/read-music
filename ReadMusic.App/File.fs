namespace ReadMusic.App

module File =

    open System.IO

    type FileReadError =
        | FileNotFound of string
        | IOError of string

    let tryReadBytes (path: string) : Result<byte[], FileReadError> =
        try
            Ok (File.ReadAllBytes path)
        with
        | :? FileNotFoundException -> Error (FileNotFound path)
        | ex -> Error (IOError ex.Message)

    let isFlac (bytes: byte[]) : bool =
        bytes.Length >= 4 &&
        bytes[0] = 0x66uy &&  // 'f'
        bytes[1] = 0x4Cuy &&  // 'L'
        bytes[2] = 0x61uy &&  // 'a'
        bytes[3] = 0x43uy     // 'C'

    let isMp3 (bytes: byte[]) : bool =
        bytes.Length >= 3 &&
        bytes[0] = 0x49uy &&  // 'I'
        bytes[1] = 0x44uy &&  // 'D'
        bytes[2] = 0x33uy     // '3'

    type AudioFormat =
        | Mp3
        | Flac
        | Unknown

    let detectFormat (bytes: byte[]) : AudioFormat =
        if isMp3 bytes then Mp3
        elif isFlac bytes then Flac
        else Unknown
