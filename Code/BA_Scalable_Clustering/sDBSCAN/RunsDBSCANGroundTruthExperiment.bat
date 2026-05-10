set D=1024
set k=2
set m=2000
set epsilon=%1
set minPts=50
set pathtoout=C:\Users\kassa\Desktop\bachelor\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\out
set inputdata=C:\Users\kassa\Desktop\bachelor\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\mnist_train.csv
set clusteroutput=C:\Users\kassa\Desktop\bachelor\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\out\mnist_train_sDBSCAN_Clusters.csv
set statsoutput=C:\Users\kassa\Desktop\bachelor\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\out\mnist_train_statsoutput.csv
set normalisedDataRaw=\normalisedDataMnistTrain.csv


cd ..\sDBSCAN\
dotnet run "%inputdata%" "%clusteroutput%" "%statsoutput%" "%D%" "%k%" "%m%" "%epsilon%" "%minPts%"
cd .\python\
echo %pathtoout% %statsoutput% %inputdata% %normalisedDataRaw% %clusteroutput% %epsilon% %minPts% | uv run groundtruth_nmi.py
cd ..
