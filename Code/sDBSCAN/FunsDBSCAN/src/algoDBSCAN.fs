module sDBSCAN.algoDBSCAN

open System
open FSharpx.Collections

let rec scalar (a:double list) (b:double list) : double =
    match a, b with
    | [], [] -> 0.0
    | x::xs, y::ys -> x*y+scalar xs ys
    | _, _ -> failwith "Vectors must have same length!"
    
let length v: double =
    let rec squaredsum = function
        | [] -> 0.0
        | x::xs -> x*x + squaredsum xs
    Math.Sqrt (squaredsum v)

let normalise v=
    let vlen = length v
    let rec aux =
        function
        | [] -> []
        | x::xs -> x / vlen :: aux xs
    aux v
    