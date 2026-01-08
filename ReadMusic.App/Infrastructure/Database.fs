module ReadMusic.App.Infrastructure.Database

open Microsoft.Data.Sqlite
open ReadMusic.App.Domain
open Dapper

let private connectionString = "Data Source=music.db;Mode=ReadWriteCreate"

let private getConnection () =
    let conn = new SqliteConnection(connectionString)
    conn.Open()
    use cmd = new SqliteCommand("PRAGMA busy_timeout = 5000", conn)
    cmd.ExecuteNonQuery() |> ignore
    conn

let ensureSchema () =
    use conn = getConnection()
    let sql = """
        CREATE TABLE IF NOT EXISTS tracks (
            id          INTEGER PRIMARY KEY AUTOINCREMENT,
            path        TEXT NOT NULL UNIQUE,
            number      TEXT,
            container   TEXT NOT NULL,
            tag_types   TEXT NOT NULL,
            extension   TEXT NOT NULL,
            title       TEXT,
            artist      TEXT,
            album       TEXT,
            year        TEXT
        )
    """
    SqlMapper.Execute(conn, sql) |> ignore
    

let insertTrack (track: Track) =
    use conn = getConnection()
   
    SqlMapper.Execute(conn, """
        INSERT OR IGNORE INTO tracks 
        (path, number, container, tag_types, extension, title, artist, album, year)
        VALUES (@Path, @Number, @Container, @TagTypes, @Extension, @Title, @Artist, @Album, @Year)
        """,
        {| 
            Path = track.Path
            Number = track.Number |> Option.defaultValue null
            Container = track.Container
            TagTypes = track.TagTypes.ToString()
            Extension = track.Extension
            Title = track.Metadata.Title |> Option.defaultValue null
            Artist = track.Metadata.Artist |> Option.defaultValue null
            Album = track.Metadata.Album |> Option.defaultValue null
            Year = track.Metadata.Year |> Option.defaultValue null
        |}) |> ignore
    