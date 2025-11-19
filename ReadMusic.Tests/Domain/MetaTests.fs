module ReadMusic.Tests.Domain.MetaTests

open Xunit
open ReadMusic.App.Domain

let private asciiBytes (s: string) : byte[] =
    System.Text.Encoding.ASCII.GetBytes s

[<Fact>]
let ``detect returns Mp3 for file starting with ID3`` () =
    let bytes = asciiBytes "ID3"
    let result = Meta.detect bytes
    Assert.Equal(Mp3, result)


[<Fact>]
let ``detect returns Flac for file starting with fLaC`` () =
    let bytes = asciiBytes "fLaC"              
    let result = Meta.detect bytes
    Assert.Equal(Flac, result)

[<Fact>]
let ``detect returns Ogg for file starting with OggS`` () =
    let bytes = asciiBytes "OggS"
    let result = Meta.detect bytes
    Assert.Equal(Ogg, result)

[<Fact>]
let ``detect returns Unknown for test`` () =
    let bytes = asciiBytes "test"
    let result = Meta.detect bytes
    Assert.Equal(Unknown, result)

[<Fact>]
let ``detect works with full file header (not just signature)`` () =
    let bytes = asciiBytes "fLaC1234"              
    let result = Meta.detect bytes
    Assert.Equal(Flac, result)