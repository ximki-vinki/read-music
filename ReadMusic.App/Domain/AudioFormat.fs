namespace ReadMusic.App.Domain

type AudioFormat =
    | Mp3
    | Flac
    | Ogg
    | Unknown

type TrackMetadata = {
    Title: string option
    Artist: string option
    Album: string option
    Year: int option
}

type Track = {
    Number: int option
    Path: string 
    Format: AudioFormat
    Metadata: TrackMetadata
}