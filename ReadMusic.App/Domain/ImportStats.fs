module ReadMusic.App.Domain.ImportStats

open ReadMusic.App.Infrastructure.Counter

type ImportStats = {
    Success: Counter
    Skipped: Counter
}

module ImportStats =
    let zero = { Success = Counter.zero; Skipped = Counter.zero }
    
    let incrementSuccess stats = 
        { stats with Success = Counter.increment stats.Success }
    
    let incrementSkipped stats = 
        { stats with Skipped = Counter.increment stats.Skipped }
