module ReadMusic.App.Infrastructure.Counter

type Counter(onUpdate: unit -> unit) =
    let mutable value = 0

    member _.Value = value
    member _.Increment() =
        value <- value + 1
        onUpdate()
