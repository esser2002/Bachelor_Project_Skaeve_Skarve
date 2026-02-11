module sDBSCAN.algoDBSCAN

open FSharpx.Collections

type node = {
    label: int
    vector: int list
}

type nodecomparison = {
    scalar: int
    comparator: node
}

let rec scalar a b =
    match a, b with
    | [], [] -> 0
    | x::xs, y::ys -> x*y+scalar xs ys
    | _, _ -> failwith "Vectors must have same length!!!"
    
let fillPriorityQueue X a =
    let rec aux X a (q:IPriorityQueue<nodecomparison>) =
        if (a > 0) then
            match X with
            | x::xs -> aux xs (a-1) (PriorityQueue.insert x q)
        else
            (X, q)
    aux X a (PriorityQueue.empty false)

let getclosest x amount (D:node list) =
    let rec aux D1 (q:IPriorityQueue<nodecomparison>) =
        match D1, q.Pop () with
        | [], _ -> q
        | j::js, (minElem, qTail) when j > minElem -> aux js (qTail.Insert j)
        | _::js, _ -> aux js q
    let (d2, q) = fillPriorityQueue (List.map (fun a -> {scalar = scalar x.vector a.vector; comparator = a}) D) amount
    aux d2 q |> Seq.toList |> List.map(fun a -> a.comparator)
    
///<param name="X">the set of all vectors</param>
///<param name="D">the set of random vectors ri</param>
///<param name="k">k nearest and furthest random vectors</param>
///<param name="m">minPoints ish</param>
let preprocessing (X:node list) (D:node list) k m =
    failwith "not implemented"