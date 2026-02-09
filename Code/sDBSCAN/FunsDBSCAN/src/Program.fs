module program
open System.Collections.Generic
open FSharp.Data
open FSharpx.Collections
open sDBSCAN.algoDBSCAN

let newnode x =
    {label = 10-x; vector = [x;11-x]}

(*
Source - https://stackoverflow.com/a/50543507
Posted by FoggyFinder
Retrieved 2026-02-09, License - CC BY-SA 4.0
*)
let readData (path : string) seps =
    CsvFile.Load(path, seps).Rows
    |> Seq.map
        (fun row -> row.Columns.[0], row.Columns |> Array.skip 1 |> Array.map int |> Array.toList)
    |> Seq.map
        (fun (title, digits) -> {label = (int) title; vector = digits})

[<EntryPoint>]
let main(args) =
    let data = readData args[0] ","
    for node in data |> Seq.truncate 10 do
        printfn $"{node.label}, {node.vector}"
    let abekat = getclosest {label = -1; vector = [5;5]} 3 [newnode 9; newnode 2; newnode 7; newnode 4; newnode 5; newnode 10; newnode 3; newnode 1; newnode 6; newnode 8]
    printfn $"{abekat |> Seq.toList}"
    0
