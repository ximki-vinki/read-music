namespace ReadMusic.App.Domain

type TrackMetadata = {
    Title: string option
    Artist: string option
    Album: string option
    Year: string option
}

type Track = {
    Number: string option
    Path: string
    FileSize: int64
    FileModifiedAt: string
    Container: string
    TagTypes: TagLib.TagTypes
    Extension: string  
    Metadata: TrackMetadata
}