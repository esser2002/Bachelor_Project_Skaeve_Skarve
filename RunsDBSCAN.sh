#!/bin/zsh 
D="10" 
k="10"
m="10"
epsilon="0.11" 
minPts="2"
inputdata="/Users/mariehansen/Desktop/Bachelor/Bachelor_Project_Skaeve_Skarve/Code/BA_Scalable_Clustering/data/mnist_train.csv"
clusteroutput="/Users/mariehansen/Desktop/Bachelor/Bachelor_Project_Skaeve_Skarve/Code/BA_Scalable_Clustering/data/out/mnist_train_cluster_sDBSCAN.csv"
statsoutput="/Users/mariehansen/Desktop/Bachelor/Bachelor_Project_Skaeve_Skarve/Code/BA_Scalable_Clustering/data/out/mnist_sDBSCAN_train_stats.csv"
normalisedDataRaw="/Users/mariehansen/Desktop/Bachelor/Bachelor_Project_Skaeve_Skarve/Code/BA_Scalable_Clustering/data/out/normalisedDataMnistTrain.csv"

cd Code/BA_Scalable_Clustering/sDBSCAN
dotnet run "$inputdata" "$clusteroutput" "$statsoutput" "$D" "$k" "$m" "$epsilon" "$minPts" 
cd python
echo "$normalisedDataRaw $clusteroutput $epsilon $minPts" | uv run nmi.py