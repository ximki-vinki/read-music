namespace ReadMusic.App.Domain

type FileData = {
    Path: string
    FileSize: int64
    FileModifiedAt: string
    Extension: string
}

type TrackMetadata = {
    Title: string option
    Artist: string option
    Album: string option
    Year: string option
    Number: string option
    TagTypes: TagLib.TagTypes
    Container: string
}

type Track = {
    FileData: FileData
    Metadata: TrackMetadata
}