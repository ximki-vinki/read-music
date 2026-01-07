module ReadMusic.App.Infrastructure.Database

open Microsoft.Data.Sqlite

let private connectionString = "Data Source=music.db;Mode=ReadWriteCreate"

let private getConnection () =
    let conn = new SqliteConnection(connectionString)
    conn.Open()
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
    Dapper.SqlMapper.Execute(conn, sql) |> ignore