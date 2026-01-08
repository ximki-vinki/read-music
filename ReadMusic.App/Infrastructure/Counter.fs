module ReadMusic.App.Infrastructure.Counter

type Counter = {
    Value: int
}

module Counter =
    let zero = { Value = 0 }
    
    let increment counter = 
        { counter with Value = counter.Value + 1 }
