open FSharp.Data

type node = {
    label: int;
    vector: int array
}

(*
Source - https://stackoverflow.com/a/50543507
Posted by FoggyFinder
Retrieved 2026-02-09, License - CC BY-SA 4.0
*)
let readData (path : string) seps =
    CsvFile.Load(path, seps).Rows
    |> Seq.map
        (fun row -> row.Columns.[0], row.Columns |> Array.skip 1 |> Array.map int)
    |> Seq.map
        (fun (title, digits) -> {label = (int) title; vector = digits})

[<EntryPoint>]
let main(args) =
    let data = readData args[0] ","
    for node in data |> Seq.truncate 10 do
        printfn $"{node.label}, {node.vector}"   
    0
