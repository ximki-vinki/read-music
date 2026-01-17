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
    use conn = getConnection ()

    let sql =
        """
        CREATE TABLE IF NOT EXISTS tracks (
            id                INTEGER PRIMARY KEY AUTOINCREMENT,            
            path              TEXT NOT NULL,
            file_size         INTEGER NOT NULL,
            file_modified_at  TEXT NOT NULL,            
            container         TEXT NOT NULL,
            tag_types         TEXT NOT NULL,
            extension         TEXT NOT NULL,
            number            TEXT,
            title             TEXT,
            artist            TEXT,
            album             TEXT,
            year              TEXT,
            created_at        TEXT NOT NULL DEFAULT (strftime('%Y-%m-%dT%H:%M:%SZ', 'now', 'utc'))
        );
        CREATE INDEX IF NOT EXISTS idx_path_created ON tracks(path, created_at DESC);
    """

    SqlMapper.Execute(conn, sql) |> ignore


let insertTrack (track: Track) =
    use conn = getConnection()
    SqlMapper.Execute(conn, """
        INSERT INTO tracks (
            path, file_size, file_modified_at,
            container, tag_types, extension,
            number, title, artist, album, year
        ) VALUES (
            @Path, @FileSize, @FileModifiedAt,
            @Container, @TagTypes, @Extension,
            @Number, @Title, @Artist, @Album, @Year
        )
        """,
        {| 
            Path = track.Path  
            FileSize = track.FileSize
            FileModifiedAt = track.FileModifiedAt
            Container = track.Container
            TagTypes = track.TagTypes.ToString()
            Extension = track.Extension
            Number = track.Number |> Option.defaultValue null
            Title = track.Metadata.Title |> Option.defaultValue null
            Artist = track.Metadata.Artist |> Option.defaultValue null
            Album = track.Metadata.Album |> Option.defaultValue null
            Year = track.Metadata.Year |> Option.defaultValue null
        |}) |> ignore