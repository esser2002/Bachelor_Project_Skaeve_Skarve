module sDBSCAN.algoDBSCAN

open FSharpx.Collections

let rec scalar a b =
    match a, b with
    | [], [] -> 0
    | x::xs, y::ys -> x*y+scalar xs ys
    | _, _ -> failwith "Vectors must have same length!!!"
    

